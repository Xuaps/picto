module Picto.Sclera

open HtmlAgilityPack.FSharp
open FSharp.Data

type Pictos() =
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
        Pictos.getCategoriesListPage
        |> Async.RunSynchronously
        |> Pictos.getCategories

    static member getPictos pictosPage =
        pictosPage
        |>createDoc
        |>descendants "h3"
        |>Seq.filter (hasClass "picto-name")
        |>Seq.map (fun h->{title:innerText h; images=followingSibling "div" h
                                        |> descendants "img"
                                        |> Seq.map (fun i-> attr "src" i)})


