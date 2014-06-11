module Picto.Sclera

open HtmlAgilityPack.FSharp
open FSharp.Data

type Categories() =
    static member getCategoriesListPage  =
        "http://www.sclera.be/en/picto/cat_overview" 
        |> Http.AsyncRequestString

    static member getCategories categoriesPage =
        categoriesPage
        |> createDoc
        |> descendants "ul"
        |> Seq.filter (hasClass "clearfix")
        |> Seq.head
        |> descendants "a"
        |> Seq.map(fun a->[|innerText a;attr "href" a|])
        |> Seq.toArray


    static member categories =
        Categories.getCategoriesListPage
        |> Async.RunSynchronously
        |> Categories.getCategories