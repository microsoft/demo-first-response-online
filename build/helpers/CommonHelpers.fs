module PlainConcepts.CommonHelpers

open System
open System.IO
open System.Linq
open System.Text.RegularExpressions
open Fake
open Fake.RestorePackageHelper
open Fake.ZipHelper


type ProjectStructureParams = {
    ArtifactsPath : string
    SourcePath : string
    BuildPath : string
    OutputPath : string
    ToolsPath : string
}

type FileItem = {
    ItemFilename : string
    ItemPath : string
    ItemFolder : string
}

type ManifestContentParams = {
    VersionName: string
    VersionCode: string
    AppId: string
}

type SolutionParams = {
    ProjectStructure : ProjectStructureParams
    SolutionItem: FileItem
}

let ProjectStructureDefaults = {
    ArtifactsPath = "artifacts/"
    SourcePath = "./"
    BuildPath = "build/"
    OutputPath = "output/"
    ToolsPath = "tools/"
}

let ManifestContentDefaults = {
    VersionName = ""
    VersionCode = ""
    AppId = ""
}

let EmptyFileItem = {
    ItemFilename = ""
    ItemPath = ""
    ItemFolder = ""
}

let SolutionDefaults = {
    ProjectStructure = ProjectStructureDefaults
    SolutionItem = EmptyFileItem
}

let (|?) = defaultArg

let GetFirstDirPathBySearchingRecursively (basePath: string) (dirName: string) =
    let filePath = Directory.EnumerateDirectories(basePath, dirName, SearchOption.AllDirectories).FirstOrDefault()
    if filePath = null then
        failwithf "dirName: %s not found in %s" dirName basePath
    filePath

let GetFirstFilenamePathBySearchingRecursively (basePath: string) (filename: string) =
    let filePath = Directory.EnumerateFiles(basePath, filename, SearchOption.AllDirectories).FirstOrDefault()
    if filePath = null then
        failwithf "filename: %s not found in %s" filename basePath
    filePath

let InitDefaultSolution (solutionFilenameArg: string) (projectStructureParams: ProjectStructureParams) = 
    let solutionFilename = solutionFilenameArg
    let solutionPath = GetFirstFilenamePathBySearchingRecursively projectStructureParams.SourcePath solutionFilename
    let solutionFolder = Path.GetDirectoryName(solutionPath)

    let solutionItem = {
        ItemFilename = solutionFilename
        ItemPath = solutionPath
        ItemFolder = solutionFolder
    }

    let defaultParameters = {
        ProjectStructure = projectStructureParams
        SolutionItem = solutionItem
    } 
    defaultParameters

let AutoFieldsRestoreMSSolutionParams (solutionParams: SolutionParams) (defaults: RestorePackageParams) = 
    let projectPath = Path.Combine(solutionParams.ProjectStructure.ToolsPath, "nuget", "nuget.exe")
    let outputPath = Path.Combine(solutionParams.SolutionItem.ItemFolder, "packages")
    {
        defaults with
            ToolPath = projectPath
            OutputPath = outputPath
    }

let NugetRestore (setParams: (RestorePackageParams) -> (RestorePackageParams)) (solutionParams: SolutionParams) =
    let restoreMSSolutionParameters param = setParams (AutoFieldsRestoreMSSolutionParams solutionParams param)
    RestoreMSSolutionPackages restoreMSSolutionParameters solutionParams.SolutionItem.ItemPath


let Nuget3Restore (setParams: (RestorePackageParams) -> (RestorePackageParams)) (solutionParams: SolutionParams) =
    let restoreMSSolutionParameters param = setParams (AutoFieldsRestoreMSSolutionParams solutionParams param)
    let solutionFile = solutionParams.SolutionItem.ItemPath

    let (parameters:RestorePackageParams) = RestorePackageDefaults |> setParams

    let sources = parameters.Sources |> buildSources

    let args = 
        "\"restore\" \"" + (solutionFile |> FullName) + "\"" + sources

    runNuGetTrial parameters.Retries parameters.ToolPath parameters.TimeOut args (fun () -> failwithf "Package restore of %s failed" solutionFile)


let UploadAllArtifactsToTeamCity (projectStructureParams: ProjectStructureParams) =
    let folders = Directory.EnumerateDirectories(projectStructureParams.ArtifactsPath, "*", SearchOption.TopDirectoryOnly)
    for folder in folders do
        let filesInFolder = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories)
        let folderFileInfo = new DirectoryInfo(folder)
        let zipFilename = folderFileInfo.Name + ".zip"
        let zipLocation = Path.Combine(projectStructureParams.ArtifactsPath, zipFilename)
        Zip folder zipLocation filesInFolder
    let files = Directory.EnumerateFiles(projectStructureParams.ArtifactsPath, "*", SearchOption.TopDirectoryOnly)
    for file in files do
        let fileFullPath = Path.GetFullPath(file)
        PublishArtifact fileFullPath
        

let CleanStructure (projectStructure: ProjectStructureParams) =
    if(Directory.Exists(projectStructure.OutputPath)) then
        Directory.Delete(projectStructure.OutputPath, true)
    if(Directory.Exists(projectStructure.ArtifactsPath)) then
        Directory.Delete(projectStructure.ArtifactsPath, true)

let RestoreWaveTools (projectStructure: ProjectStructureParams) (packageName: string) =
    let nugetConfig = Path.Combine(projectStructure.SourcePath ,"NuGet.config")
    let nuget = Path.Combine(projectStructure.ToolsPath, "nuget", "nuget.exe") |> FullName
    let nugetArgs = "install " + packageName + " -ExcludeVersion -NoCache -OutputDirectory " + projectStructure.ToolsPath + " -ConfigFile " + nugetConfig

    ExecProcessRedirected (fun p ->
        p.FileName <- nuget
        p.Arguments <- nugetArgs) (TimeSpan.FromMinutes 5.0) |> ignore

