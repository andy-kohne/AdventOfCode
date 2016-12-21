<Query Kind="FSharpProgram">
  <Output>DataGrids</Output>
</Query>

let path = Path.Combine [| Path.GetDirectoryName Util.CurrentQueryPath; ".."; "day2.txt" |]

let boxes = File.ReadAllLines path	
			|> Array.filter ((<>)"")
			|> Array.map (fun b -> b.Split([|'x'|], StringSplitOptions.RemoveEmptyEntries) |> Array.map int)

let sides (box : int[]) = [| box.[0]*box.[1]; box.[1]*box.[2]; box.[2]*box.[0] |] 
			
let part1 = boxes
			|> Array.map sides
			|> Array.sumBy (fun (b:int[]) -> Array.min b + 2 * (Array.sum b))

printfn "Part1: %A" part1

let part2 = boxes
			|> Array.sumBy (fun (b:int[]) ->
				let perimeters = [| b.[0]+b.[1]; b.[1]+b.[2]; b.[2]+b.[0] |]
				let volume = b.[0] * b.[1] * b.[2]
				Array.min perimeters * 2 + volume)

printfn "Part2: %A" part2