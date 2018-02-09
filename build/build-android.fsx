#r @"../tools/FAKE/tools/FakeLib.dll"

#load @"helpers/CommonHelpers.fs"
#load @"helpers/AndroidHelpers.fs"
#load @"helpers/XamarinHelpers.fs"

open Fake
open System
open System.IO
open System.Linq
open System.Text.RegularExpressions
open PlainConcepts.CommonHelpers
open PlainConcepts.AndroidHelpers
open PlainConcepts.XamarinHelpers
open Fake.XamarinHelper
open System.Diagnostics
open HockeyAppHelper

let buildName = (environVarOrNone "VersionName") |? "1.0"
let buildVersion = (environVarOrNone "BUILD_NUMBER") |? "1"

let configuration = "Release"

let xamarinComponentsEmail = "testcalada@gmail.com"
let xamarinComponentsPassword = "t3st1fy;2221A"

let signParams = (fun (defaults:AndroidSignAndAlignParams) ->
    {
        defaults with
            KeystorePassword = "f1rs70"
            KeystoreAlias = "FRO"
    })

let hockeyAppApiToken = "f75f5bce872d4c2ebf9fff74fddbbead"
let hockeyAppId = "fc3ab019b39d49d2869dfeeb56d2adc1"

//let structure = ProjectStructureDefaults
let structure = {
    ArtifactsPath = "artifacts/"
    SourcePath = "./"
    BuildPath = "build/"
    OutputPath = "output/"
    ToolsPath = "tools/"
}
let projectParams = InitAndroidProject "MSCorp.FirstResponse.Client.Droid.csproj" "FRO.keystore" structure
let solutionParams = InitDefaultSolution "MSCorp.FirstResponse.Client.Droid.sln" structure

let BuildProject (configuration: string) =
    let manifestChange = (fun (defaults:ManifestContentParams) ->
        {
            defaults with
                VersionName = buildName
                VersionCode = buildVersion
        })

    let androidPackageParams = (fun (defaults:AndroidPackageParams) -> 
        {
            defaults with
                Configuration = configuration
        })

    MSBuild structure.OutputPath "Clean" [("Configuration", configuration)] [ solutionParams.SolutionItem.ItemPath ] |> ignore
    AndroidPackages androidPackageParams projectParams manifestChange

let NugetRestore () =
    let customRestorePackage = (fun (defaults: RestorePackageParams) ->  
        {
            defaults with
                Retries = 3
        })
    
    NugetRestore customRestorePackage solutionParams |> ignore
    XamarinComponentRestore xamarinComponentsEmail xamarinComponentsPassword solutionParams |> ignore

let BuildAndMoveArtifactsAndroid () =
    let fileInfos = BuildProject configuration
    for fileInfo in fileInfos do
        let signFileInfo = AndroidPackageSign signParams projectParams fileInfo
        MoveAndroidArtifact projectParams signFileInfo buildVersion

let UploadBuildToHockey () =
    let apk = GetFirstFilenamePathBySearchingRecursively structure.ArtifactsPath "*.apk"
    let hokeyParams = (fun (defaults: HockeyAppUploadParams) ->  
        {
            defaults with
                ApiToken = hockeyAppApiToken
                AppId = hockeyAppId
                File = apk
                Notes = buildVersion
                DownloadStatus = DownloadStatusOption.Downloadable
                Notify = NotifyOption.All
        })

    HockeyApp hokeyParams |> ignore

Target "ci" (fun () ->

    NugetRestore ()

    BuildProject configuration |> ignore
)


Target "cd" (fun () ->

    NugetRestore ()

    BuildAndMoveArtifactsAndroid ()

    UploadAllArtifactsToTeamCity structure

    UploadBuildToHockey ()
)

RunTargetOrDefault "ci"