<Query Kind="FSharpProgram">
  <Output>DataGrids</Output>
</Query>

let path = Path.Combine [| Path.GetDirectoryName Util.CurrentQueryPath; ".."; "day1.txt" |]
let input filepath = File.ReadAllText filepath

let part1 = input path
			|> Seq.fold (fun n x ->
				match x with
				| '(' -> n+1
				| ')' -> n-1
				| _ -> n) 0
			
printfn "Part1: %A" part1

let part2 = input path
			|> Seq.scan (fun n x ->
				match x with
				| '(' -> n+1
				| ')' -> n-1
				| _ -> n) 0
			|> Seq.findIndex (fun n -> n = -1)

printfn "Part2: %A" part2
