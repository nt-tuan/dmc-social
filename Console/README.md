#### Add posts from file

```
DATABASE_URL="Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3" FILE="/Users/admin/Downloads/Medium_AggregatedData.csv" dotnet run add-posts
```

#### Calculate words

##### Environments

- DATABASE_URL: require
- START_ID: start post id, max if no defined
- END_ID: end post id, min if no defined

```
DATABASE_URL="Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3" CHUCK_SIZE=1000 dotnet run calc-words
```

#### Calculate correlation coefficient

```
DATABASE_URL="Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3" dotnet run calc-tag-correlation
```

#### Calculate tag popularity

```
DATABASE_URL="Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3" dotnet run calc-tag-popularity
```

#### Calculate post popularity

```
DATABASE_URL="Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3" dotnet run calc-post-popularity
```

#### Rank posts and tags

```
DATABASE_URL="Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3" dotnet run rank
```
