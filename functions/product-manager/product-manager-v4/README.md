# Product Manager Version 4

## Getting Started

### Open Project

```bash
# navigate to product manager v4 folder
cd azure-dotnet-examples/functions/product-manager/product-manager-v4/

# open product manager v4 using Visual Studio Code
code .
```

### Run Project

It's easy to run the project using Visual Studio or Visual Studio Code by using the "Run" or "Run and Debug" feature. Alternatively, one can use the Azure Function Core tools as follows:

```bash
# start functions using Azure Function Core Tools
cd azure-dotnet-examples/functions/product-manager/product-manager-v4/src/ProductManagerFncAppV4
func start
```

### Test Project

The easiest way to send requests to the products API is to use the [Visual Studio Code Rest Client] that allows one to send http requests directly from Visual Studio Code.

The product API requests can be found at the following location:

```yaml

product-manager-v4/fabric/api-tests/products-api.http

```

---

[Visual Studio Code Rest Client]: https://marketplace.visualstudio.com/items?itemName=humao.rest-client