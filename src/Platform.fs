module Platform

open System.IO

let readAllText (filePath: string) =
    File.ReadAllText(filePath, System.Text.Encoding.UTF8)

let writeAllText (filePath: string) (text: string) =
    File.WriteAllText(filePath, text)

let getDirFiles (path: string) (extension: string) =
    if not (Directory.Exists(path)) then [||]
    else Directory.GetFiles(path, "*" + extension, SearchOption.AllDirectories)
    |> Array.map (fun x -> x.Replace('\\', '/'))
    |> Array.sort

let ensureDirExists (path: string) =
    Directory.CreateDirectory(path) |> ignore

let getDirectoryName (path: string) =
    Path.GetDirectoryName(path)

let getFileName (path: string) =
    Path.GetFileName(path)

let getRelativePath (path: string) (pathTo: string) =
    Path.GetRelativePath(path, pathTo)

let combinePaths (path1: string) (path2: string) =
    Path.Combine(path1, path2)

let changeExtension (path: string) (ext: string) =
    Path.ChangeExtension(path, ext)
