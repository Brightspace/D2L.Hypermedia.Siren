# D2L.Hypermedia.Siren

[![Build status](https://ci.appveyor.com/api/projects/status/y8148wpqct6ao236?svg=true)](https://ci.appveyor.com/project/Brightspace/d2l-hypermedia-siren)
[![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/D2L.Hypermedia.Siren)

Helper classes for using [Siren Hypermedia](https://github.com/kevinswiber/siren) in .NET.

## Usage

`D2L.Hypermedia.Siren` fully supports Siren Hypermedia. For more information, see the link above.

```csharp
ISirenEntity entity = new SirenEntity(
	links: new [] {
		new SirenLink( rel: "self", href: new Uri( "http://example.com" )
	},
	actions: new [] {
		new SirenAction(
			name: "my-action",
			method: "GET",
			href: new Uri( "http://example.com/action" ),
			fields: new [] {
				new SirenField( name: "my-field", type: "number", value: 1 )
			}
		)
	},
	title: "Entity title",
	...
);
```

### Extension methods and SirenMatchers

There are numerous extension methods which allow you to extract a Siren representation from its parent, e.g. 

```csharp
ISirenAction action;
sirenEntity.TryGetActionByName( "my-action", out action );
```

In addition to these, `SirenMatchers` is a static class which contains a set of `Matches()` functions. These are mostly intended to be used in testing, and they will return true if the `expected` Siren representation is contained within/matches the `actual` Siren representation, e.g.

```csharp
ISirenEntity expectedEntity = new SirenEntity(
	title: "Entity title",
	actions: new [] {
		new SirenAction( name: "my-action" )
	}
);

string message;
Assert.IsTrue( SirenMatchers.Matches( expectedEntity, actualEntity, out message ), message );
```

This will verify that `actualEntity` at least contains what was specified in `expectedEntity`. If a match is not found, `message` will identify where it differs.

> **Note**: Currently, `SirenMatchers` will not properly check Siren Properties. This is because `ISirenEntity.Properties` is `dynamic`, which is difficult to test equality in a reliable on. A future version may change this, but until then, properties must be manually checked for equality.

## Contributing

1. **Fork** the repository. Committing directly against this repository is
   highly discouraged.

2. Make your modifications in a branch, updating and writing new tests.

3. Ensure that all tests pass.

4. `rebase` your changes against master. *Do not merge*.

5. Submit a pull request to this repository. Wait for tests to run and someone
   to chime in.

## Releasing

1. Merge pull request to master

2. Create release in GitHub, with a semver name of the format "v1.2.3"

3. Appveyor will run on the new tag, and upload the new version to NuGet automatically
