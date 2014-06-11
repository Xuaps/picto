namespace picto_downloader
open System
open NUnit.Framework
open Picto.Sclera

[<TestFixture>]
type ScleraTest() = 

    [<Test>]
    member x.GetCategories() =
        Assert.AreSame(["Abstract";"Animals";], getCategories)

