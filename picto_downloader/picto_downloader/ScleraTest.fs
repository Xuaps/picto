namespace picto_downloader
open System
open NUnit.Framework
open Picto.Sclera
open FSharp.Data
open HtmlAgilityPack.FSharp

[<TestFixture>]
type ScleraTest() = 

    [<Test>]
    member x.GetCategories() =
        Assert.AreEqual([|[|"Abstract";"/cat/1"|];[|"Animals";"/cat/2"|]|], Categories.getCategories "<html><head></head><body><ul class='clearfix'><li><a href='/cat/1'>Abstract</a></li><li><a href='/cat/2'>Animals</a></li></ul></body></html>")
    