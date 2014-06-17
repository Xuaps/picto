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
        Assert.AreEqual([|[|"Abstract";"/cat/1"|];[|"Animals";"/cat/2"|]|], Pictos.getCategories "<html><head></head><body><ul class='clearfix'><li><a href='/cat/1'>Abstract</a></li><li><a href='/cat/2'>Animals</a></li></ul></body></html>")
    
    [<Test>]
    member x.GetPictos() =
        let pictos = Pictos.getPictos "<html><head></head><body><div class='picto-by-name-list'><h3 class='picto-name'>seasons</h3><div class='group'><a href='/en/picto/detail/21353' class='picto-tTitle><span><img src='/resources/pictos/seizoenen%20t.png'></span></a></div><h3 class='picto-name'>sunday</h3><div class='group'><a href='/en/picto/detail/19083' class='picto-thumb'><span><img src='/resources/pictos/pellenberg/zondag%20t.png'></span></a><a href='/en/picto/detail/19047' class='picto-thumb'><span><img src='/resources/pictos/kleur/zondag blauw%20t.png'></span></a></div></div></body></html>"
        Assert.AreEqual("sunday", pictos.[1].Tittle)
        Assert.AreEqual([|"/resources/pictos/pellenberg/zondag.png";"/resources/pictos/kleur/zondag blauw.png"|], pictos.[1].Images)
    