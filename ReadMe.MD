# An XPO Implementation of IDataView (a data source for machine learning)

This example is a port of the exmple published by microsoft on how to implement the
[IDataView](https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.idataview?view=ml-dotnet)

### How to get started with ML.NET 




[Training Machine Learning models with ML.NET](https://www.youtube.com/watch?v=HZOuPsJJFl0)(
 This is an episode of the show "On .NET" and also first video you will get if you write ML.NET  in youtube searchbox)

[ML.NET - Machine Learning Introduction | ML.NET [1 of 8]](https://www.youtube.com/watch?v=X0DQjfW09kA)

[ML.NET - Machine Machine Learning simplified for Developers with ML.NET - Jernej "JK" Kavka](https://youtu.be/LG1DHMNT0TA)
[Bri Achtman (Microsoft Project Manager)](https://devblogs.microsoft.com/dotnet/author/brachtmamicrosoft-com/)

### How to use the XpoInputObjectDataView (M.L IDataView)

```csharp
            ///Create an XPView
            XPView xpView = new XPView(Session.DefaultSession, typeof(InputObjectXpo));


            ///Define your label and your text properties
            const string Lable = nameof(InputObjectXpo.Label);
            const string Text = nameof(InputObjectXpo.Text);
            xpView.Properties.AddRange(new ViewProperty[]
            {
                    new ViewProperty(Lable, SortDirection.None, $"[{Lable}]", true, true),
                    new ViewProperty(Text, SortDirection.None, $"[{Text}]", true, true),
            });


            ///Create an instance of XpoInputObjectDataView and pass the xpView and the name of the text and label property
            var dataxpView = new XpoInputObjectDataView(xpView, Text, Lable);

            //Use the instance of XpoInputObjectDataView with your MLContext 
            // So, this is a very simple pipeline: a transformer that tokenizes
            // Text, does nothing with the Label column at all.
            var mlContext = new MLContext();

            var transformedDataView = mlContext.Transforms.Text.TokenizeIntoWords(
                "TokenizedText", "Text").Fit(dataxpView).Transform(dataxpView);
```