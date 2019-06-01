module DownloadAsync

open System.Text.RegularExpressions
open System.Net
open System.IO
open System

/// Cкачивает все веб-страницы, на которые есть ссылки с указанной, и печатает информацию о размере каждой 
let downloadAsync url = 
    let fetchAsync (url : string) = 
        printfn"%s" url
        async {        
            try 
                let request = WebRequest.Create(url)   
                use! response = request.AsyncGetResponse()
                use stream = response.GetResponseStream()
                use reader = new StreamReader(stream) 
                let html = reader.ReadToEnd()
                do printfn "%s --- %d..." url html.Length 
                return Some html
            with 
            | _ -> printfn "Ooops, something is wrong"
                   return None
        }
    let pagesForDownload html = 
        let regex = new Regex("<a href\s*=\s*\"?(https?://[^\"]+)\"?\s*>")  //("<a\s+(?:[^>]*?\s+)?href=\"([^\"]*)")
        let matches = regex.Matches(html)
        matches |> Seq.map (fun (x : Match) -> x.Groups.[1].Value) 
                |> Seq.map fetchAsync |> Async.Parallel |> Async.RunSynchronously
    let workflow = fetchAsync url
    let pages = workflow |> Async.RunSynchronously  
    match pages with 
    |None -> [None]
    |Some x -> pages :: ((pagesForDownload x) |> Array.toList)

    
