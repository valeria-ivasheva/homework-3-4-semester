/// Задача 3.2 (map для деревьев)
module MapForTree

/// АТД Бинарное дерево
type Tree<'a> = 
    | Tree of 'a * Tree<'a> * Tree<'a>
    | Tip

/// Функция, применяющая функцию func к каждому элементу двоичного дерева tree
let rec mapTree tree (func : 'a -> 'a )= 
    match tree with
    | Tree(a, l, r) -> Tree(func a, mapTree l func, mapTree r func)
    | Tip -> Tip 
