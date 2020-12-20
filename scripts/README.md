#### Add posts from file

```
dotnet run add-posts "Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3" "/Users/admin/Downloads/articles.csv"
```

#### Calculate words

##### Environments

- DATABASE_URL: require
- START_ID: start post id, max if no defined
- END_ID: end post id, min if no defined

```
DATABASE_URL="Host=localhost;Database=social;Username=social;Password=gkwgkGKgkkkgkgkakjgkjahgka" END_ID=38031 dotnet run calc-words
```
