# Code generation of Records and Unions for C# 8

Install the package from NuGet: [Laconic.CodeGeneration](https://www.nuget.org/packages/Laconic.CodeGeneration/0.9.3-beta). This is compile-time only dependency.

## Records

Write an interface marked with `RecordsAttribute`, the interface name doesn't matter:

```csharp
[Records]
public interface MyRecords
{
    record User(string firstName, string lastName);
}
```

**What's generated:** each method in the interface becomes an immutable partial class, with a constructor, value equality and `With` method. Each parameter in the method becomes a property:

```csharp
var johnny = new User("Johnny", "Smith");
Console.WriteLine(johnny.FirstName); // prints "Johnny"
Console.WriteLine(johhny == new User("Johnny", "Smith")); // prints "true"

var grownUp = johnny.With(firstName: "John"); // values that are not supplied are copied from the original object
Console.WriteLine(johnny == grownUp); // prints "false"
```

## Unions

Write an interface marked with `UnionAttribute` using one or more underscores as suffix or prefix.

```csharp
[Union]
    interface __Shape__
    {
        record Circle(double radius);
        record Rectangle(double length, double width);
    }
```

**What's generated:** The name of the interface with stripped underscores becomes the name of your union, each method becomes a record implementing the union interface:

```csharp
var circle = new Circle(10);
var rect = new Rectangle(10, 20);

double Area(Shape shape) => shape switch {
    Circle c => Math.PI * c.Radius * c.Radius,
    Rectangle r => r.Length * r.Height,
    _ => throw new NotImplementedException()
};
```

## Credits

The inspiration (and some code) came from the excellent [LanguageExt](https://github.com/louthy/language-ext) project. Check it out!

All the heavy lifting is done by [CodeGeneration.Roslyn](https://github.com/AArnott/CodeGeneration.Roslyn).
