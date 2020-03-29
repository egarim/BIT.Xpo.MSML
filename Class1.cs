using BIT.Xpo.MSML;
using System;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Samples.Dynamic
{
    /// <summary>
    /// The <see cref="IDataView"/> interface is the central concept of "data" in
    /// ML.NET. While many conveniences exist to create pre-baked implementations,
    /// it is also useful to know how to create one completely from scratch. We also
    /// take this opportunity to illustrate and motivate the basic principles of how
    /// the IDataView system is architected, since people interested in
    /// implementing <see cref="IDataView"/> need at least some knowledge of those
    /// principles.
    /// </summary>
    public static partial class SimpleDataViewImplementation
    {
        //public static void Example()
        //{
        //    // First we create an array of these objects, which we "present" as this
        //    // IDataView implementation so that it can be used in a simple ML.NET
        //    // pipeline.
        //    var inputArray = new[]
        //    {
        //        new InputObject(false, "Hello my friend."),
        //        new InputObject(true, "Stay awhile and listen."),
        //        new InputObject(true, "Masterfully done hero!")
        //    };
        //    var dataView = new InputObjectDataView(inputArray);

        //    // So, this is a very simple pipeline: a transformer that tokenizes
        //    // Text, does nothing with the Label column at all.
        //    var mlContext = new MLContext();
        //    var transformedDataView = mlContext.Transforms.Text.TokenizeIntoWords(
        //        "TokenizedText", "Text").Fit(dataView).Transform(dataView);

        //    var textColumn = transformedDataView.Schema["Text"];
        //    var tokensColumn = transformedDataView.Schema["TokenizedText"];

        //    using (var cursor = transformedDataView.GetRowCursor(
        //        new[] { textColumn, tokensColumn }))

        //    {
        //        // Note that it is best to get the getters and values *before*
        //        // iteration, so as to facilitate buffer sharing (if applicable),
        //        // and column-type validation once, rather than many times.
        //        ReadOnlyMemory<char> textValue = default;
        //        VBuffer<ReadOnlyMemory<char>> tokensValue = default;

        //        var textGetter = cursor
        //            .GetGetter<ReadOnlyMemory<char>>(textColumn);

        //        var tokensGetter = cursor
        //            .GetGetter<VBuffer<ReadOnlyMemory<char>>>(tokensColumn);

        //        while (cursor.MoveNext())
        //        {
        //            textGetter(ref textValue);
        //            tokensGetter(ref tokensValue);

        //            Console.WriteLine(
        //                $"{textValue} => " +
        //                $"{string.Join(", ", tokensValue.DenseValues())}");

        //        }

        //        // The output to console is this:

        //        // Hello my friend. => Hello, my, friend.
        //        // Stay awhile and listen. => Stay, awhile, and, listen.
        //        // Masterfully done hero! => Masterfully, done, hero!

        //        // Note that it may be interesting to set a breakpoint on the
        //        // Console.WriteLine, and explore what is going on with the cursor,
        //        // and the buffers. In particular, on the third iteration, while
        //        // `tokensValue` is logically presented as a three element array,
        //        // internally you will see that the arrays internal to that
        //        // structure have (at least) four items, specifically:
        //        // `Masterfully`, `done`, `hero!`, `listen.`. In this way we see a
        //        // simple example of the details of how buffer sharing from one
        //        // iteration to the next actually works.
        //    }
        //}

       
    }
}