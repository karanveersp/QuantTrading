/// The models used to describe trading data, not speaking in terms of a specific API or data source.
module TradingData.Model

open System

type CryptoSymbol =
    | BTCUSD
    | ETHUSD

type TimeFrame =
    | Minute
    | FifteenMinute
    | OneHour
    | FourHour
    | Day
    | Week

type OHLCBar =
    { Symbol: string
      Open: decimal
      Close: decimal
      Low: decimal
      High: decimal
      Volume: decimal
      TimeUtc: DateTime
      Trades: uint64
      VolumeWeightedAveragePrice: decimal }
