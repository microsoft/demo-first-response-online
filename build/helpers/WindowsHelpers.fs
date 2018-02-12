module PlainConcepts.WindowsHelpers

open System
open System.IO
open System.Linq
open System.Text.RegularExpressions
open PlainConcepts.CommonHelpers
open Fake
open Fake.MSBuildHelper
open Fake.Testing.NUnit3
open Fake.AssemblyInfoFile


type WindowsStructureParams = {
    ProjectStructure : ProjectStructureParams
    ProjectItem : FileItem
    ManifestItem : FileItem
    ProjectOutput : string
}

let WindowsStructureDefaults = {
    ProjectStructure = ProjectStructureDefaults
    ProjectItem = EmptyFileItem
    ManifestItem = EmptyFileItem
    ProjectOutput = ""
}

let InitDefaultWindowsProject (projectFilenameArg: string) (projectStructure: ProjectStructureParams) = 
    let projectFilename = projectFilenameArg
    let projectPath = GetFirstFilenamePathBySearchingRecursively projectStructure.SourcePath projectFilename
    let projectFolder = Path.GetDirectoryName(projectPath)
    
    let manifestFilename = "AssemblyInfo.cs"
    let manifestFolder = Path.Combine(projectFolder, "Properties")
    let manifestPath = Path.Combine(manifestFolder, manifestFilename)
    
    let projectName = Path.GetFileNameWithoutExtension(projectFilename)
    let projectOutput = Path.Combine(projectStructure.OutputPath, projectName)

    let projectItem = {
        ItemFilename = projectFilename
        ItemPath = projectPath
        ItemFolder = projectFolder
    }

    let manifestItem = {
        ItemFilename = manifestFilename
        ItemPath = manifestPath
        ItemFolder = manifestFolder
    }

    let defaultParameters = {
        ProjectStructure = projectStructure
        ProjectItem = projectItem
        ManifestItem = manifestItem
        ProjectOutput = projectOutput
    }
    defaultParameters


let InitDefaultUWPProject (projectFilenameArg: string) (projectStructure: ProjectStructureParams) = 
    let projectFilename = projectFilenameArg
    let projectPath = GetFirstFilenamePathBySearchingRecursively projectStructure.SourcePath projectFilename
    let projectFolder = Path.GetDirectoryName(projectPath)
    
    let manifestFilename = "Package.appxmanifest"
    let manifestPath = Path.Combine(projectFolder, manifestFilename)
    
    let projectName = Path.GetFileNameWithoutExtension(projectFilename)
    let projectOutput = Path.Combine(projectStructure.OutputPath, projectName)

    let projectItem = {
        ItemFilename = projectFilename
        ItemPath = projectPath
        ItemFolder = projectFolder
    }

    let manifestItem = {
        ItemFilename = manifestFilename
        ItemPath = manifestPath
        ItemFolder = projectFolder
    }

    let defaultParameters = {
        ProjectStructure = projectStructure
        ProjectItem = projectItem
        ManifestItem = manifestItem
        ProjectOutput = projectOutput
    }
    defaultParameters


let AutoFieldsNUnitParams (windowsParams: WindowsStructureParams) (defaults: NUnit3Params) = 
    let projectPath = Path.Combine(windowsParams.ProjectStructure.ToolsPath, "NUnit.ConsoleRunner", "tools", "nunit3-console.exe")
    let resultSpecs = Path.Combine(windowsParams.ProjectOutput, "TestResult.xml")
    {
        defaults with
            ToolPath = projectPath
            ResultSpecs = [resultSpecs]
    }


let WindowsUpdateManifest (setParams: (ManifestContentParams) -> (ManifestContentParams)) (windowsParams: WindowsStructureParams) =
    let changeWindowsManifestProperty (attribute:Attribute) =
        printfn "Updating %s from %s to: %s" attribute.Name windowsParams.ManifestItem.ItemFilename attribute.Value

        UpdateAttributes windowsParams.ManifestItem.ItemPath [attribute]

        let updatedManifestContent = File.ReadAllText(windowsParams.ManifestItem.ItemPath)
        printfn "Updated %s: %s" windowsParams.ManifestItem.ItemFilename updatedManifestContent

    let updateManifest (manifestContentParams: ManifestContentParams) (windowsParams: WindowsStructureParams) =
        if manifestContentParams.VersionName <> "" then
            let attribute = Attribute.FileVersion(manifestContentParams.VersionName)
            changeWindowsManifestProperty attribute
        if manifestContentParams.VersionCode <> "" then
            let attribute = Attribute.Version(manifestContentParams.VersionCode)
            changeWindowsManifestProperty attribute
        if manifestContentParams.AppId <> "" then
            let attribute1 = Attribute.Title(manifestContentParams.AppId)
            let attribute2 = Attribute.Product(manifestContentParams.AppId)
            changeWindowsManifestProperty attribute1
            changeWindowsManifestProperty attribute2

    let updateManifestParams = setParams ManifestContentDefaults
    updateManifest updateManifestParams windowsParams


let UWPUpdateManifest (setParams: (ManifestContentParams) -> (ManifestContentParams)) (windowsParams: WindowsStructureParams) =
    let changeUWPManifestProperty (property:string) (value:string) =
        let manifestPath = windowsParams.ManifestItem.ItemPath
        printfn "Updating %s from %s to: %s" property manifestPath value

        let manifestContent = File.ReadAllText(manifestPath)    
        let updatedManifestContent = Regex.Replace(manifestContent, " " + property + "=\"([^\"]+)\"", " " + property + "=\"" + value + "\"")    
        File.WriteAllText(manifestPath, updatedManifestContent)

        printfn "Updated %s: %s" manifestPath updatedManifestContent

    let updateUWPManifest (manifestContentParams: ManifestContentParams) (windowsParams: WindowsStructureParams) =
        if manifestContentParams.VersionName <> "" then
            changeUWPManifestProperty "Version" manifestContentParams.VersionName

    let updateManifestParams = setParams ManifestContentDefaults
    updateUWPManifest updateManifestParams windowsParams


let WindowsBuild (windowsParams: WindowsStructureParams) (setManifestContentParams: (ManifestContentParams) -> (ManifestContentParams)) targets properties =
    let manifestBackupPath = windowsParams.ManifestItem.ItemPath + ".BAK"

    File.Copy(windowsParams.ManifestItem.ItemPath, manifestBackupPath, true)

    try
        WindowsUpdateManifest setManifestContentParams windowsParams
        MSBuild windowsParams.ProjectOutput targets properties [windowsParams.ProjectItem.ItemPath]
    finally
        File.Copy(manifestBackupPath, windowsParams.ManifestItem.ItemPath, true)
        File.Delete(manifestBackupPath)

let UWPBuild (windowsParams: WindowsStructureParams) (setManifestContentParams: (ManifestContentParams) -> (ManifestContentParams)) targets properties =
    let manifestBackupPath = windowsParams.ManifestItem.ItemPath + ".BAK"

    File.Copy(windowsParams.ManifestItem.ItemPath, manifestBackupPath, true)

    try
        UWPUpdateManifest setManifestContentParams windowsParams
        MSBuild windowsParams.ProjectOutput targets properties [windowsParams.ProjectItem.ItemPath]
    finally
        File.Copy(manifestBackupPath, windowsParams.ManifestItem.ItemPath, true)
        File.Delete(manifestBackupPath)


let MoveWindowsArtifact (projectParams: WindowsStructureParams) (appendToArtifactName: string) =
    let projectOutput = projectParams.ProjectOutput
    let projectOutputFileInfo = new DirectoryInfo(projectOutput)
    let projectOutputFolderName = projectOutputFileInfo.Name + "_" + appendToArtifactName
    let projectOutputArtifact = Path.Combine(projectParams.ProjectStructure.ArtifactsPath, projectOutputFolderName)
    
    if (not (Directory.Exists(projectParams.ProjectStructure.ArtifactsPath))) then
        Directory.CreateDirectory(projectParams.ProjectStructure.ArtifactsPath) |> ignore

    if (Directory.Exists(projectOutputArtifact)) then
        Directory.Delete(projectOutputArtifact, true)
    Directory.Move(projectOutput, projectOutputArtifact)

let MoveUWPArtifact (projectParams: WindowsStructureParams) (appendToArtifactName: string) =
    let projectOutput = Path.Combine(projectParams.ProjectItem.ItemFolder, "AppPackages")
    let projectOutputArtifact = Path.Combine(projectParams.ProjectStructure.ArtifactsPath, "UWP_" + appendToArtifactName)
    
    if (not (Directory.Exists(projectParams.ProjectStructure.ArtifactsPath))) then
        Directory.CreateDirectory(projectParams.ProjectStructure.ArtifactsPath) |> ignore

    if (Directory.Exists(projectOutputArtifact)) then
        Directory.Delete(projectOutputArtifact, true)

    Directory.Move(projectOutput, projectOutputArtifact)

let NUnit3Run (setNUnit3Params: (NUnit3Params) -> (NUnit3Params)) (windowsParams: WindowsStructureParams) (searchPattern: string) =
    let NUnitParameters param = setNUnit3Params (AutoFieldsNUnitParams windowsParams param)

    let testDll = GetFirstFilenamePathBySearchingRecursively windowsParams.ProjectOutput searchPattern

    NUnit3 NUnitParameters [testDll]


let RestoreWindowsWaveTools (projectStructure: ProjectStructureParams) =
    let packageName = "WaveEngine.WindowsTools"
    RestoreWaveTools projectStructure packageName

    let waveTools = Path.Combine(projectStructure.ToolsPath, packageName) |> FullName
    let target = Path.Combine(waveTools, "v2.0", "Tools", "VisualEditor")

    !! (waveTools @@ "tools" @@ "*.*")
        |> CopyFiles target

    setEnvironVar "WaveEngine" (waveTools + @"\")

    let path = "PATH"
    let environVarPath = environVar path
    let customEnvironVarPath = waveTools + ";" + environVarPath
    setEnvironVar path customEnvironVarPath