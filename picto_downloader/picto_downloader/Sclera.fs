module Picto.Sclera

open HtmlAgilityPack.FSharp
open FSharp.Data

let getCategoriesListPage  =
    "http://www.sclera.be/en/picto/cat_overview" 
    |> Http.AsyncRequestString

let getCategories categoriesPage=
    categoriesPage
    |> createDoc
    |> descendants "ul"
    |> Seq.filter (hasClass "clearfix")
    |> Seq.head
    |> followingSibling "li"
    |> descendants "a"
    |> Seq.head
    |> innerText



