
MEF Calculator Extended
=======


Summary
-----

In this example, I made minor modifications to the calculator example to refresh its directory catalog if an invalidOperationException is thrown. The proof of concept is that if a caller expects a behavior and it is not there, we can attempt to refresh our dependencies and try again. Obviously, a mechanism should be put into place such that more than 1-2 failures should be properly logged and thrown.



Normal Instructions (regarding MEF calc sample)
------

To compile and run this project, you will need to specify the path to the Extensions folder.

1. Open the main code file (Module1.vb or Program.cs).
2. In the constructor, specify the path to the Extensions folder on your local computer.
    catalog.Catalogs.Add(New DirectoryCatalog("C:\\Users\\SomeUser\\Documents\\Visual Studio 2010\\Projects\\SimpleCalculator2\\SimpleCalculator2\\Extensions"))


To test this project, perform the following steps:

1 - Run the project. - A command window appears.

2 - Type addition or subtraction commands.
```
    1+1
    1-2
```
3 - Try to type multiplication or division commands.
```
    1*2
    1/2
```
4 - The following message is displayed.
```
    Operation not found!
```
