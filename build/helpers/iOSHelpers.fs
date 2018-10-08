module PlainConcepts.iOSHelpers

open System
open System.IO
open System.Linq
open System.Text.RegularExpressions
open PlainConcepts.CommonHelpers
open Fake
open Fake.XamarinHelper


type iOSStructureParams = {
    ProjectStructure : ProjectStructureParams
    ProjectItem : FileItem
    ManifestItem : FileItem
    ProjectOutput : string
}

let iOSStructureDefaults = {
    ProjectStructure = ProjectStructureDefaults
    ProjectItem = EmptyFileItem
    ManifestItem = EmptyFileItem
    ProjectOutput = ""
}

let AutoFieldsiOSStructure (projectFilenameArg: string) (projectStructure: ProjectStructureParams) = 
    let projectFilename = projectFilenameArg
    let projectPath = GetFirstFilenamePathBySearchingRecursively projectStructure.SourcePath projectFilename
    let projectFolder = Path.GetDirectoryName(projectPath)

    let manifestFilename = "Info.plist"
    let manifestFolder = projectFolder
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

let AutoFieldsiOSPackage (projectParams: iOSStructureParams) (defaults: iOSBuildParams) = 
    let projectPath = projectParams.ProjectItem.ItemPath
    let outputPath = (projectParams.ProjectOutput |> FullName |> trimSeparator ) + "/"
    {
        defaults with
            ProjectPath = projectPath
            Properties = [("OutputPath", outputPath)]
    }

let iOSUpdateManifest (setParams: (ManifestContentParams) -> (ManifestContentParams))  (iOSParams: iOSStructureParams) =
    let changeiOSInfoPlistProperty (property:string) (value:string) =

        printfn "Updating %s from %s to: %s" property iOSParams.ManifestItem.ItemFilename value

        let infoplistContent = File.ReadAllText(iOSParams.ManifestItem.ItemPath)
        let regexPattern = "<key>" + property + "</key>(\s+)<string>(.*?)</string>"

        if Regex.IsMatch(infoplistContent, regexPattern) then
            let newValueString = "<key>" + property + "</key>$1<string>" + value + "</string>"
            let updatedinfoplistContent = Regex.Replace(infoplistContent, regexPattern, newValueString)
            File.WriteAllText(iOSParams.ManifestItem.ItemPath, updatedinfoplistContent)
        else
            traceError("Error while updating manifest. Not match")

    let updateManifest (manifestContentParams: ManifestContentParams) =
        if manifestContentParams.VersionName <> "" then
            changeiOSInfoPlistProperty "CFBundleShortVersionString" manifestContentParams.VersionName
        if manifestContentParams.VersionCode <> "" then
            changeiOSInfoPlistProperty "CFBundleVersion" manifestContentParams.VersionCode
        if manifestContentParams.AppId <> "" then
            changeiOSInfoPlistProperty "CFBundleIdentifier" manifestContentParams.AppId

    let updateManifestParams = setParams ManifestContentDefaults

    updateManifest updateManifestParams


let iOSPackage (setParams: (iOSBuildParams) -> (iOSBuildParams)) (iOSParams: iOSStructureParams) (setManifestContentParams: (ManifestContentParams) -> (ManifestContentParams)) =
    let iOSBuildParameters param = setParams (AutoFieldsiOSPackage iOSParams param)

    let manifestBackupPath = iOSParams.ManifestItem.ItemPath + ".BAK"

    File.Copy(iOSParams.ManifestItem.ItemPath, manifestBackupPath, true)

    try
        iOSUpdateManifest setManifestContentParams iOSParams
        iOSBuild iOSBuildParameters
    finally
        File.Copy(manifestBackupPath, iOSParams.ManifestItem.ItemPath, true)
        File.Delete(manifestBackupPath)


let MoveiOSArtifacts (projectParams: iOSStructureParams) (appendToArtifactName: string) =
    let MoveArtifact (projectOutput: string) (searchPattern: string) = 
        let ipa = GetFirstFilenamePathBySearchingRecursively projectOutput searchPattern
        let ipaFilename = Path.GetFileName(ipa)
        let ipaArtifact = Path.Combine(projectParams.ProjectStructure.ArtifactsPath, ipaFilename)

        if (File.Exists(ipaArtifact)) then
            File.Delete(ipaArtifact)
        File.Move(ipa, ipaArtifact)

    let MoveArtifactDir (projectOutput: string) (searchPattern: string) = 
        let dSYMExtension = ".dSYM"
        let appdSYM = GetFirstDirPathBySearchingRecursively projectOutput searchPattern
        let appdSYMFolder = (new DirectoryInfo(appdSYM)).Name
        let appdSYMFilename = appdSYMFolder.Substring(0, appdSYMFolder.Length - dSYMExtension.Length) + "_" + appendToArtifactName + dSYMExtension
        let appdSYMArtifact = Path.Combine(projectParams.ProjectStructure.ArtifactsPath, appdSYMFilename)
    
        if (Directory.Exists(appdSYMArtifact)) then
            Directory.Delete(appdSYMArtifact, true)
        Directory.Move(appdSYM, appdSYMArtifact)

    let artifactFolder = projectParams.ProjectStructure.ArtifactsPath
    if (not (Directory.Exists(artifactFolder))) then
        Directory.CreateDirectory(artifactFolder) |> ignore

    MoveArtifact projectParams.ProjectOutput "*.ipa"
    MoveArtifactDir projectParams.ProjectOutput "*.app.dSYM"


let RestoreMacWaveTools (projectStructure: ProjectStructureParams) =
    let packageName = "WaveEngine.MacTools"

    RestoreWaveTools projectStructure packageName

    let waveTools = Path.Combine(projectStructure.ToolsPath, packageName) |> FullName
    let target = Path.Combine(waveTools, "v2.0", "Tools", "VisualEditor")

    let chmodArgs = "+x " + waveTools + "/tools/sox"

    ExecProcessRedirected (fun p ->
        p.FileName <- "chmod"
        p.Arguments <- chmodArgs) (TimeSpan.FromMinutes 5.0) |> ignore

    let target = "/Library/Frameworks/WaveEngine.framework/v2.0/Tools/VisualEditor/"
    !! (waveTools @@ "tools" @@ "*.*")
        |> CopyFiles target

let RunSigh (appleIdUser: string) (appleIdPass: string) (args: string) =
    setEnvironVar "FASTLANE_USER" appleIdUser
    setEnvironVar "FASTLANE_PASSWORD" appleIdPass

    ExecProcessRedirected (fun p ->
        p.FileName <- "sigh"
        p.Arguments <- args) (TimeSpan.FromMinutes 5.0) |> ignore

let UpdateDevProvisionings (appleIdUser: string) (appleIdPass: string) (packageName: string) (provisioningName: string) =
    let args = "-a '" + packageName + "' --development -n '" + provisioningName + "'"
    RunSigh appleIdUser appleIdPass args


let UpdateDistProvisionings (appleIdUser: string) (appleIdPass: string) (packageName: string) (provisioningName: string) =
    let args = "-a '" + packageName + "' -n '" + provisioningName + "'"
    RunSigh appleIdUser appleIdPass args
