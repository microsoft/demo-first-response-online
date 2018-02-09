#Get Data
serverData = RxSqlServerData(sqlQuery = "SELECT * FROM dbo.Dataset", connectionString = "SERVER=[SERVER];DATABASE=Seattle911;UID=[USER];PWD=[PASSWORD];")
incidences = rxImport(serverData)

#Clean Missing Data
incidences = incidences[incidences$Rain <= 1,]

#Select Columns in dataset
incidences = data.frame(incidences$Distance1000, incidences$ZoneId, incidences$Distance3000, incidences$Distance6000, incidences$DistancePlus6000, incidences$Temperature, incidences$SeaLvlPress, incidences$Windspeed, incidences$Rain, incidences$EventCount)

#Normalize Data: Z scores
incidences <- incidences[, apply(incidences, 2, var) != 0]

for (i in seq_len(ncol(incidences)-1))
    incidences[, i] <- scale(incidences[, i])

#Split data
smp_size <- floor(0.60 * nrow(incidences)) # 60% of the sample size

train_ind <- sample(seq_len(nrow(incidences)), size = smp_size)

train <- incidences[train_ind,]
test <- incidences[-train_ind,]

#Desition forest (using RevoScaleR functions) which are available in Open R 3.3.3 (included in AzureSQL for R)
model <- rxDForest(incidences.EventCount ~ incidences.Distance1000 + incidences.Distance3000 + incidences.Distance6000 + incidences.DistancePlus6000 + incidences.Temperature + incidences.SeaLvlPress + incidences.Windspeed, data = train, maxDepth = 14, nTree = 14, cp = 0.01)
plot(model)

#Predict histogram
hist(rxPredict(model, test, type = "response")[[1]])

#Predict value
sub = test[98,]
rxPredict(model, sub, type = "response")[[1]]