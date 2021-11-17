---
page_type: sample
languages:
- C#
products:
- Azure
- Azure Storage
- Aszure Storage Table
description: "A .NET 6 API that demonstrates how to use Azure.Data.Tables SDK"
---

# Table Storage - CRUD API Example

A .NET 6 API that demonstrates how to use Azure.Data.Tables SDK.

The following concepts are demonstrated:

- Single Table Design
- Game Reviews Endpoint (POST, PUT, GET, DELETE)
- Game Review Summary Endpoint (POST, PUT, GET, DELETE)

![storage-table-crud-1](https://user-images.githubusercontent.com/33935506/142160973-eaae966d-e608-4eb6-98db-281c343747a5.png)

---

## Getting Started

Please see [Azure Storage Queue - Getting Started] guide where I explain how to get started with Azure storage using both an emulator (Azurite) and the actual Azure Storage Service.

### Configure Connection String

```powershell
# configure connection string to use local emulator

dotnet user-secrets set "ConnectionStrings:ReviewData" "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
```

---

## Resources

Use the following templates to generate sample batch input data for game reviews and game review summaries.

Copy relevant template and generate the JSON using <https://www.json-generator.com>

### Generate Game Reviews

```json
[
  '{{repeat(20)}}',
  {
    userId: '{{guid()}}',
    userDisplayName: '{{firstName()}} {{surname()}}',
    gameId: '{{guid()}}',
    title: '{{lorem(3, "words")}}',
    platform: '{{random("PS5", "XBoxSeriesX", "NinetendoSwitch", "PC")}}',
    rating: '{{integer(3,10)}}',
    review: '{{lorem(1, "paragraphs")}}'
  }
]
```

### Generate Game Review Summaries

```json
[
  '{{repeat(20)}}',
  {
    gameId: '{{guid()}}',
    platform: '{{random("PS5", "XBoxSeriesX", "NinetendoSwitch", "PC")}}',
    title: '{{lorem(3, "words")}}',
    description: '{{lorem(1, "paragraphs")}}',
    engine: '{{lorem(1, "words")}}',
    series: '',
    coverArtLink: 'http://placehold.it/32x32',
    coverArtThumbnailLink: 'http://placehold.it/32x32',
    averageUserRating: '{{floating(3, 10).toFixed(2)}}',
    developers: [
      '{{repeat(1, 3)}}',
      '{{lorem(1, "words")}}'
    ],
    "publishers": [
      '{{repeat(1, 3)}}',
      '{{lorem(1, "words")}}'
    ],
    "genres": [
      '{{repeat(1, 3)}}',
      '{{lorem(1, "words")}}'
    ]
  }
]
```

---

[Azure Storage Queue - Getting Started]: https://github.com/drminnaar/azure-dotnet-examples/blob/main/storage/README.md