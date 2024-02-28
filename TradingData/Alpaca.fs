/// All the Alpaca API specific functions and type mappings are defined in this module.
module TradingData.Alpaca

open System
open System.Threading.Tasks
open Alpaca.Markets
open Alpaca.Markets.Extensions

open FSharp.Control
open TradingData.Model


let toAlpacaTimeFrame (tf: TimeFrame): BarTimeFrame =
    match tf with
    | Minute -> BarTimeFrame.Minute
    | FifteenMinute -> BarTimeFrame(15, BarTimeFrameUnit.Minute)
    | OneHour -> BarTimeFrame.Hour
    | FourHour -> BarTimeFrame(4, BarTimeFrameUnit.Hour)
    | Day -> BarTimeFrame.Day
    | Week -> BarTimeFrame.Week

let fromAlpacaBar (bar: IBar): OHLCBar =
    {
        Symbol = bar.Symbol
        Open = bar.Open
        Close = bar.Close
        High = bar.High
        Low = bar.Low
        Volume = bar.Volume
        TimeUtc = bar.TimeUtc
        Trades = bar.TradeCount
        VolumeWeightedAveragePrice = bar.Vwap
    }

let toAlpacaSymbol (symbol: CryptoSymbol): string =
    match symbol with
    | BTCUSD -> "BTC/USD"
    | ETHUSD -> "ETH/USD"


let getLiveCryptoDataClient (key: string) (secret: string) : IAlpacaCryptoDataClient =
    SecretKey(key, secret)
    |> Environments.Live.GetAlpacaCryptoDataClient

let getPaperCryptoDataClient (key: string) (secret: string) : IAlpacaCryptoDataClient =
    SecretKey(key, secret)
    |> Environments.Paper.GetAlpacaCryptoDataClient

let getHistoricalCryptoData (client: IAlpacaCryptoDataClient) (symbol: CryptoSymbol) (from: DateTime) (into: DateTime) (timeFrame: TimeFrame): Task<OHLCBar list> =
    let req =
        HistoricalCryptoBarsRequest(
            symbol |> toAlpacaSymbol,
            from,
            into,
            timeFrame |> toAlpacaTimeFrame)

    task {
        return!
            client.GetHistoricalBarsAsAsyncEnumerable(req)
            |> AsyncSeq.ofAsyncEnum
            |> AsyncSeq.map fromAlpacaBar
            |> AsyncSeq.toListAsync
    }
