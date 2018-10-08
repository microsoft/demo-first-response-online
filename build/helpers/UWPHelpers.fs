module PlainConcepts.UWPHelpers

open System
open System.IO
open System.Linq
open System.Text.RegularExpressions
open PlainConcepts.CommonHelpers
open Fake
open Fake.MSBuildHelper
open Fake.Testing.NUnit3
open Fake.AssemblyInfoFile


type UWPStructureParams = {
    ProjectStructure : ProjectStructureParams
    ProjectItem : FileItem
    ManifestItem : FileItem
    ProjectOutput : string
}

let UWPStructureDefaults = {
    ProjectStructure = ProjectStructureDefaults
    ProjectItem = EmptyFileItem
    ManifestItem = EmptyFileItem
    ProjectOutput = ""
}

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


let UWPUpdateManifest (setParams: (ManifestContentParams) -> (ManifestContentParams)) (windowsParams: UWPStructureParams) =
    let changeUWPManifestProperty (property:string) (value:string) =
        let manifestPath = windowsParams.ManifestItem.ItemPath

        printfn "Updating %s from %s to: %s" property manifestPath value

        let manifestContent = File.ReadAllText(manifestPath)    
        let updatedManifestContent = Regex.Replace(manifestContent, " " + property + "=\"([^\"]+)\"", " " + property + "=\"" + value + "\"")    
        File.WriteAllText(manifestPath, updatedManifestContent)

        printfn "Updated %s: %s" manifestPath updatedManifestContent

    let updateUWPManifest (manifestContentParams: ManifestContentParams) (windowsParams: UWPStructureParams) =
        if manifestContentParams.VersionName <> "" then
            changeUWPManifestProperty "Version" manifestContentParams.VersionName

    let updateManifestParams = setParams ManifestContentDefaults
    updateUWPManifest updateManifestParams windowsParams


let UWPBuild (windowsParams: UWPStructureParams) (setManifestContentParams: (ManifestContentParams) -> (ManifestContentParams)) targets properties =
    let manifestBackupPath = windowsParams.ManifestItem.ItemPath + ".BAK"

    File.Copy(windowsParams.ManifestItem.ItemPath, manifestBackupPath, true)

    try
        UWPUpdateManifest setManifestContentParams windowsParams
        MSBuild windowsParams.ProjectOutput targets properties [windowsParams.ProjectItem.ItemPath]
    finally
        File.Copy(manifestBackupPath, windowsParams.ManifestItem.ItemPath, true)
        File.Delete(manifestBackupPath)


let MoveUWPArtifact (projectParams: UWPStructureParams) (appendToArtifactName: string) =
    let projectOutput = Path.Combine(projectParams.ProjectItem.ItemFolder, "AppPackages")
    
    if (not (Directory.Exists(projectParams.ProjectStructure.ArtifactsPath))) then
        Directory.CreateDirectory(projectParams.ProjectStructure.ArtifactsPath) |> ignore

    let packageFolder = Directory.EnumerateDirectories(projectOutput, "*_Test", SearchOption.TopDirectoryOnly).FirstOrDefault()
    let filesInFolder = Directory.EnumerateFiles(packageFolder, "*", SearchOption.AllDirectories)
    let folderFileInfo = new DirectoryInfo(packageFolder)
    let zipFilename = folderFileInfo.Name + ".zip"
    let zipLocation = Path.Combine(projectParams.ProjectStructure.ArtifactsPath, zipFilename)
    Zip packageFolder zipLocation filesInFolder