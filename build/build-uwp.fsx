#r @"../tools/FAKE/tools/FakeLib.dll"

#load @"helpers/CommonHelpers.fs"
#load @"helpers/UWPHelpers.fs"

open Fake
open System
open System.IO
open System.Linq
open System.Diagnostics
open System.Text.RegularExpressions
open PlainConcepts.CommonHelpers
open PlainConcepts.UWPHelpers
open Fake.RestorePackageHelper
open Fake.XamarinHelper
open Fake.Testing.NUnit3
open HockeyAppHelper

let buildName = (environVarOrNone "VersionName") |? "1.0.0.0"
let buildVersion = (environVarOrNone "BUILD_NUMBER") |? "1"

let structure = ProjectStructureDefaults

let configuration = "Release"

let projectParams = InitDefaultUWPProject "MSCorp.FirstResponse.Client.UWP.csproj" structure
let solutionParams = InitDefaultSolution "MSCorp.FirstResponse.Client.UWP.sln" structure

let hockeyAppApiToken = "f75f5bce872d4c2ebf9fff74fddbbead"
let hockeyAppId = "3a52af8ed05b4c4fab0489300b7a766e"

let RestoreNugets () =
    let customRestorePackage = (fun (defaults: RestorePackageParams) ->  
        {
            defaults with
                Retries = 3
        })

    Nuget3Restore customRestorePackage solutionParams

let buildProject (configuration: string) projectParams =
    let uwpManifestFirstPart = buildName.Substring(0, buildName.LastIndexOf("."))
    let uwpVersion = uwpManifestFirstPart + "." + buildVersion
    let manifestChange = (fun (defaults:ManifestContentParams) ->
        {
            defaults with
                VersionName = uwpVersion
        })

    UWPBuild projectParams manifestChange "Build" ["Configuration", configuration]

let UploadBuildToHockey () =
    let zip = GetFirstFilenamePathBySearchingRecursively structure.ArtifactsPath "*.zip"
    let hokeyParams = (fun (defaults: HockeyAppUploadParams) ->  
        {
            defaults with
                ApiToken = hockeyAppApiToken
                AppId = hockeyAppId
                File = zip
                Notes = buildVersion
                DownloadStatus = DownloadStatusOption.Downloadable
                Notify = NotifyOption.All
        })

    HockeyApp hokeyParams |> ignore

Target "ci" (fun () ->

    CleanStructure structure
    
    RestoreNugets () |> ignore

    buildProject configuration projectParams |> ignore
)

Target "cd" (fun () ->

    CleanStructure structure
    
    RestoreNugets () |> ignore

    buildProject configuration projectParams |> ignore

    MoveUWPArtifact projectParams buildVersion

    UploadAllArtifactsToTeamCity structure
    
    UploadBuildToHockey ()
)

RunTargetOrDefault "ci"