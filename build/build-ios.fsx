#r @"../tools/FAKE/tools/FakeLib.dll"

#load @"helpers/CommonHelpers.fs"
#load @"helpers/iOSHelpers.fs"
#load @"helpers/XamarinHelpers.fs"

open Fake
open System
open System.IO
open System.Linq
open System.Text.RegularExpressions
open PlainConcepts.CommonHelpers
open PlainConcepts.iOSHelpers
open PlainConcepts.XamarinHelpers
open Fake.XamarinHelper
open System.Diagnostics
open HockeyAppHelper

let buildName = (environVarOrNone "VersionName") |? "1.0"
let buildVersion = (environVarOrNone "BUILD_NUMBER") |? "1"

let configuration = "Release"

let structure = ProjectStructureDefaults
let projectParams = AutoFieldsiOSStructure "MSCorp.FirstResponse.Client.iOS.csproj" structure
let solutionParams = InitDefaultSolution "MSCorp.FirstResponse.Client.iOS.sln" structure

let xamarinComponentsEmail = "testcalada@gmail.com"
let xamarinComponentsPassword = "t3st1fy;2221A"

let hockeyAppApiToken = "f75f5bce872d4c2ebf9fff74fddbbead"
let hockeyAppId = "909691d86d4b4a8f8bea6330263c3d8f"

let NugetRestore () =
    let customRestorePackage = (fun (defaults: RestorePackageParams) ->  
        {
            defaults with
                Retries = 3
        })
    
    NugetRestore customRestorePackage solutionParams |> ignore
    XamarinComponentRestore xamarinComponentsEmail xamarinComponentsPassword solutionParams |> ignore

let BuildProject (configuration: string) =

    let manifestChange = (fun (defaults:ManifestContentParams)->
        {
            defaults with
                VersionName = buildName
                VersionCode = buildVersion
        })

    let iOSPackageParams = (fun (defaults:iOSBuildParams) -> 
        {
            defaults with
                BuildIpa = true
                Configuration = configuration
                Platform = "iPhone"
        })

    iOSPackage iOSPackageParams projectParams manifestChange |> ignore

let UploadBuildToHockey () =
    let ipa = GetFirstFilenamePathBySearchingRecursively structure.ArtifactsPath "*.ipa"
    let dsym = GetFirstFilenamePathBySearchingRecursively structure.ArtifactsPath "*.dSYM.zip"

    let hokeyParams = (fun (defaults: HockeyAppUploadParams) ->  
        {
            defaults with
                ApiToken = hockeyAppApiToken
                AppId = hockeyAppId
                File = ipa
                Dsym = dsym
                Notes = buildVersion
                DownloadStatus = DownloadStatusOption.Downloadable
                Notify = NotifyOption.All
        })

    HockeyApp hokeyParams |> ignore

Target "ci" (fun () ->
    
    NugetRestore ()

    BuildProject configuration
)

Target "cd" (fun () ->

    NugetRestore ()

    BuildProject configuration

    MoveiOSArtifacts projectParams buildVersion

    UploadAllArtifactsToTeamCity structure

    UploadBuildToHockey () 
)

RunTargetOrDefault "ci"