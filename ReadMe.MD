# An XPO Implementation of IDataView (a data source for machine learning)

This example contains an implementation of the [IDataView](https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.idataview?view=ml-dotnet)
from the ML.NET framework, the name of the class that implements the IDataView is XpoInputObjectDataView and it takes a [XpView](https://docs.devexpress.com/XPO/2068/concepts/xpview-concepts) and the name of the "Label" and and "Text" Properties as shown below

```csharp
        var MLDataSource = new XpoInputObjectDataView(xpView, "Text property name", "Label property name");
     
```
### How to get started with ML.NET 




[Training Machine Learning models with ML.NET](https://www.youtube.com/watch?v=HZOuPsJJFl0)(
 This is an episode of the show "On .NET" and also first video you will get if you write ML.NET  in youtube searchbox)

[ML.NET - Machine Learning Introduction | ML.NET [1 of 8]](https://www.youtube.com/watch?v=X0DQjfW09kA)

[ML.NET - Machine Machine Learning simplified for Developers with ML.NET - Jernej "JK" Kavka](https://youtu.be/LG1DHMNT0TA)
[Bri Achtman (Microsoft Project Manager)](https://devblogs.microsoft.com/dotnet/author/brachtmamicrosoft-com/)

### How to use the XpoInputObjectDataView (M.L IDataView)

After you have added reference to the project BIT.Xpo.MSML (nuget comming soon...) create your ML.NET IDataView by creating an instance of XpoInputObjectDataView as shown below in 3 easy steps

```csharp
            ///Step 1) Create an XPView
            XPView xpView = new XPView(Session.DefaultSession, typeof(InputObjectXpo));



            ///Step 2)Define your label and your text properties
            const string Lable = nameof(InputObjectXpo.Label);
            const string Text = nameof(InputObjectXpo.Text);
            xpView.Properties.AddRange(new ViewProperty[]
            {
                    new ViewProperty(Lable, SortDirection.None, $"[{Lable}]", true, true),
                    new ViewProperty(Text, SortDirection.None, $"[{Text}]", true, true),
            });

            //Step 2.5)Pass a criteria (this step is optional)
            //BetweenOperator betweenOperator = new BetweenOperator("Code", "002", "003");
            //xpView.Criteria = betweenOperator;

            ///Step 3)Create an instance of XpoInputObjectDataView and pass the xpView and the name of the text and label property
            var dataxpView = new XpoInputObjectDataView(xpView, Text, Lable);

            //Use the instance of XpoInputObjectDataView with your MLContext 
           
            var mlContext = new MLContext();

            var transformedDataView = mlContext.Transforms.Text.TokenizeIntoWords(
                "TokenizedText", "Text").Fit(dataxpView).Transform(dataxpView);
```

### The unit test project

there are 3 unit test in the test project

**MicrosoftExample:** This example is a port of the exmple published by microsoft on how to implement the
[IDataView](https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.idataview?view=ml-dotnet)

**XpoExample:** this is the same example as but using XpoInputObjectDataView as data source

**DifferentDataModelXpoExample:** this is again the same example but using a different XPO as data source 