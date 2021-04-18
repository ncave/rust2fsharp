module Program

open Platform

let processFile filePath outPath =
    let text = readAllText filePath
    let fsText = Patterns.replacePatterns text
    let fsPath = changeExtension outPath ".fs"
    ensureDirExists (getDirectoryName fsPath)
    writeAllText fsPath fsText
    printfn "Processed: %s" filePath

[<EntryPoint>]
let main argv =
    match argv with
    | [| rustDir; outDir|] ->
        let filePaths = getDirFiles rustDir ".rs"
        for filePath in filePaths do
            let relPath = getRelativePath rustDir filePath
            let outPath = combinePaths outDir relPath
            processFile filePath outPath
    | _ ->
        printfn "Usage: rust2fsharp rustDir outDir"
    0
