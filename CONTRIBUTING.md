# Contributing
First of all, thank you for considering contributing to Istio.Tracing.Propagation, we love contributions!

## Prerequisites
By contributing, you assert that:
* The contribution is your own original work.
* You have the right to assign the copyright for the work (it is not owned by your employer, or you have been given copyright assignment in writing).

## Code
### Code Style
We use the [normal .NET coding guidelines](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/) as a base for the coding style, with some changes such as:
* Use 4 spaces for indentation (no tabs)
* Use `camelCase` for private fiels (do not use `_` )
* Always specify member visibility, even if it's the default (i.e. `private string foo;` not `string foo;`)

### Unit Tests
Make sure to run all unit tests before creating a pull request. Any new code, including bugfixes, should have unit tests.

To build and run the unit tests, run the following command:

    dotnet test -c Release

## Contributing Process
Fork, then clone the repo:

    git clone git@github.com:your-username/istio-tracing-aspnetcore.git

[Make sure the tests pass.](#unit-tests)

Make your changes, including tests for the changes you made.

Make sure the solution builds without warnings and tests pass again.

Push to your fork and [submit a pull request](https://github.com/nosinovacao/istio-tracing-aspnetcore/compare/
).

Some things that will increase the chance that your pull request is accepted:

* Write tests.
* Follow our [code style](#code-style).
* Write a good commit message.