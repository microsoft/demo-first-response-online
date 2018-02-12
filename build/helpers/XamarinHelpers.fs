module PlainConcepts.XamarinHelpers

open System
open System.IO
open System.Linq
open System.Text.RegularExpressions
open PlainConcepts.CommonHelpers
open Fake
open Fake.RestorePackageHelper
open Fake.ZipHelper


let GetCurrentXamarinFolderTools (projectStructure: ProjectStructureParams) =
    let fullToolPath = Path.Combine(projectStructure.ToolsPath, "xamarin")
    fullToolPath

let GetCurrentCookieJar (projectStructure: ProjectStructureParams) =
    let fullXamarinPath = GetCurrentXamarinFolderTools(projectStructure)
    let fullCookieJarPath = Path.Combine(fullXamarinPath, "cookie_jar")
    fullCookieJarPath

let GetXamarinComponentExe (projectStructure: ProjectStructureParams) =
    let fullXamarinPath = GetCurrentXamarinFolderTools(projectStructure)
    let xamarinLocation = Path.Combine(fullXamarinPath, "xamarin-component.exe")
    xamarinLocation

let SetCookieJar (projectStructure: ProjectStructureParams) =
    let fullCookieJarPath = GetCurrentCookieJar projectStructure
    setEnvironVar "COOKIE_JAR_PATH" fullCookieJarPath

let RemoveCookieJarIfExists (projectStructure: ProjectStructureParams) =
    let fullCookieJarPath = GetCurrentCookieJar projectStructure
    if (File.Exists(fullCookieJarPath)) then
        File.Delete(fullCookieJarPath)

let LogInToXamarinComponent (username: string) (password: string) (projectStructure: ProjectStructureParams) =
    RemoveCookieJarIfExists projectStructure
    SetCookieJar projectStructure

    let xamarinLocation = GetXamarinComponentExe projectStructure
    let args = "login " + (quote username) + " -p " + (quote password)

    ExecProcessRedirected (fun p ->
            p.FileName <- xamarinLocation
            p.Arguments <- args) (TimeSpan.FromSeconds 10.0)
            
let XamarinComponentRestore (username: string) (password: string) (solutionParams: SolutionParams) =
    LogInToXamarinComponent username password solutionParams.ProjectStructure |> ignore

    let xamarinLocation = GetXamarinComponentExe solutionParams.ProjectStructure
    let args = "restore " + (quote solutionParams.SolutionItem.ItemPath)

    ExecProcessRedirected (fun p ->
            p.FileName <- xamarinLocation
            p.Arguments <- args) (TimeSpan.FromSeconds 10.0)
