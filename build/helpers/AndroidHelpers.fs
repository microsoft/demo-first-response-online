module PlainConcepts.AndroidHelpers

open System
open System.IO
open System.Linq
open System.Text.RegularExpressions
open PlainConcepts.CommonHelpers
open Fake
open Fake.XamarinHelper

type AndroidStructureParams = {
    ProjectStructure : ProjectStructureParams
    ProjectItem : FileItem
    ManifestItem : FileItem
    KeyStoreItem : FileItem
    ProjectOutput : string
}

let AndroidStructureDefaults = {
    ProjectStructure = ProjectStructureDefaults
    ProjectItem = EmptyFileItem
    ManifestItem = EmptyFileItem
    KeyStoreItem = EmptyFileItem
    ProjectOutput = ""
}

let InitAndroidProject (projectFilenameArg: string) (keyStoreFilenameArg: string) (projectStructure: ProjectStructureParams)  = 
    let projectFilename = projectFilenameArg
    let projectPath = GetFirstFilenamePathBySearchingRecursively projectStructure.SourcePath projectFilename
    let projectFolder = Path.GetDirectoryName(projectPath)

    let manifestFilename = "AndroidManifest.xml"
    let manifestFolder = Path.Combine(projectFolder, "Properties")
    let manifestPath = Path.Combine(manifestFolder, manifestFilename)

    let keyStoreFilename = keyStoreFilenameArg
    let keyStorePath = GetFirstFilenamePathBySearchingRecursively projectFolder keyStoreFilename
    let keyStoreFolder = Path.GetDirectoryName(keyStorePath)

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

    let keyStoreItem = {
        ItemFilename = keyStoreFilename
        ItemPath = keyStorePath
        ItemFolder = keyStoreFolder
    }

    let defaultParameters = {
        ProjectStructure = projectStructure
        ProjectItem = projectItem
        ManifestItem = manifestItem
        KeyStoreItem = keyStoreItem
        ProjectOutput = projectOutput
    }
    defaultParameters

let AutoFieldsAndroidSign (androidParams: AndroidStructureParams) (defaults: AndroidSignAndAlignParams) = 
    let zipalignPath = Path.Combine(androidParams.ProjectStructure.ToolsPath, "android", "zipalign")
    let projectFolder = androidParams.ProjectItem.ItemFolder
    let keyStorePath = androidParams.KeyStoreItem.ItemPath
    {
        defaults with
            ZipalignPath = zipalignPath
            KeystorePath = keyStorePath
    }

let AutoFieldsAndroidPackage (projectParams: AndroidStructureParams) (defaults: AndroidPackageParams) = 
    let projectPath = projectParams.ProjectItem.ItemPath
    let outputPath = (projectParams.ProjectOutput |> FullName |> trimSeparator ) + "/"
    {
        defaults with
            ProjectPath = projectPath
            OutputPath = outputPath
            Properties = [("OutputPath", outputPath)]
    }

let AndroidPackageSign (setParams:(AndroidSignAndAlignParams) -> (AndroidSignAndAlignParams)) (androidParams: AndroidStructureParams) (androidApk: FileInfo) =
    let androidSignAndAlign param = setParams (AutoFieldsAndroidSign androidParams param)

    AndroidSignAndAlign androidSignAndAlign androidApk

let AndroidUpdateManifest (setParams: (ManifestContentParams) -> (ManifestContentParams)) (androidParams: AndroidStructureParams) =
    let changeAndroidManifestProperty (property:string) (value:string) =

        printfn "Updating %s from %s to: %s" property androidParams.ManifestItem.ItemFilename value

        let manifestContent = File.ReadAllText(androidParams.ManifestItem.ItemPath)
        let regexPattern = " " + property + "=\"(.*?)\""

        if Regex.IsMatch(manifestContent, regexPattern) then
            let newValueString = " " + property + "=\"" + value + "\""
            let updatedinfopmanifestContent = Regex.Replace(manifestContent, regexPattern, newValueString)
            File.WriteAllText(androidParams.ManifestItem.ItemPath, updatedinfopmanifestContent)
        else
            traceError("Error while updating manifest. Not match")

    let updateManifest (manifestContentParams: ManifestContentParams) =
        if manifestContentParams.VersionName <> "" then
            changeAndroidManifestProperty "android:versionName" manifestContentParams.VersionName
        if manifestContentParams.VersionCode <> "" then
            changeAndroidManifestProperty "android:versionCode" manifestContentParams.VersionCode
        if manifestContentParams.AppId <> "" then
            changeAndroidManifestProperty "package" manifestContentParams.AppId

    let updateManifestParams = setParams ManifestContentDefaults

    updateManifest updateManifestParams


let AndroidPackages (setBuildParams: (AndroidPackageParams) -> (AndroidPackageParams)) (androidParams: AndroidStructureParams) (setManifestContentParams: (ManifestContentParams) -> (ManifestContentParams)) =
    let androidPackage param = setBuildParams (AutoFieldsAndroidPackage androidParams param)

    let manifestBackupPath = androidParams.ManifestItem.ItemPath + ".BAK"

    File.Copy(androidParams.ManifestItem.ItemPath, manifestBackupPath, true)

    try
        AndroidUpdateManifest setManifestContentParams androidParams
        AndroidBuildPackages androidPackage
    finally
        File.Copy(manifestBackupPath, androidParams.ManifestItem.ItemPath, true)
        File.Delete(manifestBackupPath)

let MoveAndroidArtifact (projectParams: AndroidStructureParams) (androidApkPath: FileInfo) (appendToArtifactName: string) =
    let artifactFolder = projectParams.ProjectStructure.ArtifactsPath
    let apk = androidApkPath.FullName
    let apkFilename = Path.GetFileNameWithoutExtension(apk) + "_" + appendToArtifactName + Path.GetExtension(apk)
    let apkArtifact = Path.Combine(projectParams.ProjectStructure.ArtifactsPath, apkFilename)

    if (not (Directory.Exists(artifactFolder))) then
        Directory.CreateDirectory(artifactFolder) |> ignore

    if (File.Exists(apkArtifact)) then
        File.Delete(apkArtifact)
    File.Move(apk, apkArtifact)
