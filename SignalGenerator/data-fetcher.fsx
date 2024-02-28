#r "nuget: Alpaca.Markets, 7.0.1"
#r "nuget: Alpaca.Markets.Extensions, 7.0.0"
#r "nuget: Deedle, 3.0.0"
#r "nuget: FSharp.Control.AsyncSeq"
#r "../TradingData/bin/Debug/net8.0/TradingData.dll"

open System
open Deedle

open TradingData.Model
open TradingData.Alpaca


let keyId = ""
let keySecret = ""

let client = getLiveCryptoDataClient keyId keySecret

let from = DateTime(2023, 11, 1)
let till = DateTime.UtcNow

let res =
    (getHistoricalCryptoData
        client
        BTCUSD
        from
        till
        OneHour).Result
    |> Frame.ofRecords
    |> Frame.indexRowsDate "TimeUtc"
    |> Frame.sortRowsByKey

res.SaveCsv("BTCUSD-OneHour-11-1-23-till-2-27-24.csv")

