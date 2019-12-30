using static System.Console;
using System;
using static System.Math;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;



public enum Color {
    RED, BLACK, Blue
}

public class NodeList {
    Node[] list;
    int index;

    public NodeList(int length) {
        this.list = new Node[length];
        this.index=0;
    }

    public void add(Node n) {
        this.list[index] = n;
        this.index++;
    }

    public int Length {
        get {
            return this.list.Length;
        }
    }


    public Node this[int i] {
        get { return this.list[i]; }
    }
}

public class Node {
    public int key;
    public Node left, right, parent;
    public Color color;

    public Node() {

    }

    public Node(int key) {
        this.key = key;
    }

}

class BinaryTree{
    Node root;
    public int digits=0;

    private class NIL {
        private static Node Nil;
        
        public static Node NilNode() {
            if (Nil == null) {
                Nil = new Node();
                Nil.color = Color.BLACK;
                Nil.left = Nil;
                Nil.right = Nil;
            }

            return Nil;
        }
    }

    public void insert(int z) {
        insert(ref root, z);
    }

    public void insert(ref Node n, int z) {
        if (n==null){
            n = new Node(z);
            return;
        }
        Node parent=null;
        Node curr=n;
        while (curr!=null) {
            parent=curr;
            if (z > curr.key)
                curr = curr.right;
            else if (z < curr.key)
                curr = curr.left;
            else
                return;
        }
        if (z > parent.key)
            parent.right = new Node(z);
        else
            parent.left = new Node(z);

    }

    private static Node remove_helper(Node n, int key) {
        if (n.key.CompareTo(key)==0) {
            if (n.left==null && n.right==null) {
                return null;
            } else if (n.left==null) {
                return n.right;
            } else if (n.right==null){
                return n.left;
            } else {
                //complicated method
                n.right = removeSmallest(ref n.right, out n.key);
                return n;
            }
        } else if (key.CompareTo(n.key)<0) {
            n.left = remove_helper(n.left, key);
            return n;
        } else {
            n.right = remove_helper(n.right, key);
            return n;
        }
    }


    static Node removeSmallest(ref Node n, out int key) {
        if (n.left==null && n.right==null) {
            key = n.key;
            return null;
        }
        if (n.left==null) {
            key = n.key;
            return n.right;
        }
        n.left = removeSmallest(ref n.left, out key);
        return n;

    }


    public bool delete(int key){
        Node n;
        if (!find(key, out n)) {
            return false;
        }
        this.root = remove_helper(this.root, key);
        return true;
    }

    private static bool contains_helper(Node n,int key, out Node nout) {
        if (n == null) {
            nout = null;
            return false;
        }
        if (n.key.CompareTo(key)==0) {
            nout = n;
            return true;
        }
        if (n.key.CompareTo(key) < 0) {
            return contains_helper(n.right , key, out nout);
        } else {
            return contains_helper(n.left, key, out nout);
        }
    }

    public bool find(int key, out Node n){
        return contains_helper(this.root, key, out n);
    }



    // =============== PRINT ==============
    public void print() {
        if (root==null) {
            WriteLine("Tree is empty");
            return;
        }
        NodeList[] levels = getTreePerLevels();

        int height = getHeight(this.root);
        for (int i=height+1;i>=0;i--) {
            int padding = formula(i, this.digits);
            int num = height+1-i;
            for (int j=0;j<Math.Pow(2, num);j++) {
                if (levels[num][j]!=null) {
                    if (levels[num][j]!=NIL.NilNode())
                        encapsulate(padding, j, levels[num][j].key, this.digits, levels[num][j].color);
                    else    
                        encapsulate(padding, j, int.MinValue, this.digits, Color.BLACK);
                } else {
                    encapsulate(padding, j, int.MinValue, this.digits, Color.Blue);    
                }
            }
            WriteLine();
        }
    }

    public int Height {
        get {
            return getHeight(this.root);
        }
    }

    private static int getHeight(Node n) {
        if (n==null || n==NIL.NilNode()) {
            return -1;
        }
        return 1+Max(getHeight(n.left), getHeight(n.right));
    }


    private int formula(int i, int digits) {
        return (int)((digits*Math.Pow(2,i)+digits*(Math.Pow(2,i)-1))/2-(digits/2));
    }

    static void encapsulate(int padding, int count, int keyue, int digits, Color c) {
        if (count==0) {
            if (padding!=0) {
                if (keyue!=int.MinValue) {
                    for (int i=1;i<=padding/2;i++) {
                        Write(" ");
                    }
                    for (int i=padding/2+1;i<=padding;i++) {
                        Write("-");
                    }
                } else {
                    for (int i=1;i<=padding;i++) {
                        Write(" ");
                    }
                }
            }
            if (c==Color.BLACK) {
                Console.BackgroundColor = ConsoleColor.Black;
            } else if (c==Color.RED) {
                Console.BackgroundColor = ConsoleColor.Red;
            } else {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            if (keyue==int.MinValue) {
                for (int i=0;i<digits;i++) {
                    Write(" ");
                }
            } else{
                Write(keyue.ToString("D"+digits.ToString()));
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            if (keyue!=int.MinValue) {
                for (int i=1;i<=((padding+digits/2)/2);i++) {
                    Write("-");
                }
                for (int i=(padding+digits/2)/2;i<padding+digits/2;i++) {
                    Write(" ");
                }
            } else {
                for (int i=1;i<=padding+digits/2;i++) {
                    Write(" ");
                }
            }
        } else {
            if (keyue!=int.MinValue) {
                for (int i=1;i<=(padding+digits/2)/2+1;i++) {
                    Write(" ");
                }
                for (int i=(padding+digits/2)/2+2;i<=padding+digits/2;i++) {
                    Write("-");
                }
            } else {
                for (int i=1;i<=padding+digits/2;i++) {
                    Write(" ");
                }
            }
            if (c==Color.BLACK) {
                Console.BackgroundColor = ConsoleColor.Black;
            } else if (c==Color.RED) {
                Console.BackgroundColor = ConsoleColor.Red;
            } else {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            if (keyue==int.MinValue) {
                for (int i=0;i<digits;i++) {
                    Write(" ");
                }
            } else{
                Write(keyue.ToString("D"+digits.ToString()));
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            if (keyue!=int.MinValue) {
                for (int i=1;i<=(padding+digits/2)/2;i++) {
                    Write("-");
                }
                for (int i=(padding+digits/2)/2+1;i<=padding+digits/2;i++) {
                    Write(" ");
                }
            } else {
                for (int i=1;i<=padding+digits/2;i++) {
                    Write(" ");
                }
            }
        }
    }

    private NodeList[] getTreePerLevels() {
        int height = getHeight(this.root);
        NodeList[] levels = new NodeList[height+2];
        for (int i=0;i<levels.Length;i++) {
            levels[i] = new NodeList((int)Math.Pow(2, i));
        }
        populate(ref levels, this.root, 0);
        return levels;
    }

    static void populate(ref NodeList[] levels, Node n, int level) {
        if (n==null) {
            levels[level].add(null);
            int j = 1;
            for (int i=level+1;i<levels.Length;i++) {
                for (int k=1;k<=j;k++){
                    levels[i].add(null);
                    levels[i].add(null);
                }
                j++;
            }
            return;
        }
        if (n==NIL.NilNode()) {
            levels[level].add(NIL.NilNode());
            int j = 1;
            for (int i=level+1;i<levels.Length;i++) {
                for (int k=1;k<=j;k++){
                    levels[i].add(null);
                    levels[i].add(null);
                }
                j++;
            }
            return;   
        }
        populate(ref levels, n.left, level+1);
        levels[level].add(n);
        populate(ref levels, n.right, level+1);
    }

}

public class RedBlackTree {
    public Node root = NIL.NilNode();
    public int digits = 0;

    private class NIL {
        private static Node Nil;
        
        public static Node NilNode() {
            if (Nil == null) {
                Nil = new Node();
                Nil.color = Color.BLACK;
                Nil.left = Nil;
                Nil.right = Nil;
            }

            return Nil;
        }
    }

    private int leftBlackHeight(Node n) {
        if (n == NIL.NilNode()) {
            return 1;
        }
        return n.color == Color.BLACK ? leftBlackHeight(n.left)+1 : leftBlackHeight(n.left);
    }

    private int rightBlackHeight(Node n) {
        if (n == NIL.NilNode()) {
            return 1;
        }
        return n.color == Color.BLACK ? rightBlackHeight(n.left)+1 : rightBlackHeight(n.left);
    }

    private bool checktree(Node n) {
        if (n == NIL.NilNode()) {
            return true;
        }
        if (!(checktree(n.left) && checktree(n.right))) {
            WriteLine(n.key+" HEIGHT NOT EVEN");
        }
        return (leftBlackHeight(n.left) == rightBlackHeight(n.right)) && checktree(n.left) && checktree(n.right);
    }


    private Node leftRotate(Node x) {
        Node y = x.right;
        x.right = y.left;
        if (y.left != NIL.NilNode())
            y.left.parent = x;
        y.parent = x.parent;
        if (x.parent == NIL.NilNode()) {
            root = y;
        }
        else if (x == x.parent.left) {
            x.parent.left = y;
        }
        else {
            x.parent.right = y;
        }
        y.left = x;
        x.parent = y;
        return x;
    }

    private Node rightRotate(Node x) {
        Node y = x.left;
        x.left = y.right;
        if (y.right != NIL.NilNode())
            y.right.parent = x;
        y.parent = x.parent;
        if (x.parent == NIL.NilNode()) {
            root = y;
        }
        else if (x == x.parent.right) {
            x.parent.right = y;
        }
        else {
            x.parent.left = y;
        }
        y.right = x;
        x.parent = y;
        return x;
    }

    public void insert(int key) {
        digits = Max(digits, (key.ToString().Length%2==1 ? key.ToString().Length+1 : key.ToString().Length ));
        Node newNode = new Node();
        newNode.key = key;
        newNode.left = NIL.NilNode();
        newNode.right = NIL.NilNode();
        insert(newNode);
    }

    private void insert(Node z) {
        Node y = NIL.NilNode();
        Node x = root;
        while (x != NIL.NilNode()) {
            y = x;
            if (z.key < x.key)
                x = x.left;
            else if (z.key > x.key)
                x = x.right;
            else if (z.key == x.key)
                return;
        }
        z.parent = y;
        if (y == NIL.NilNode()) {
            root = z;
        }
        else if (z.key < y.key) {
            y.left = z;
        }
        else if (z.key > y.key) {
            y.right = z;
        }
        else if (z.key == y.key) {
            return;
        }
        z.left = NIL.NilNode();
        z.right = NIL.NilNode();
        z.color = Color.RED;
        z = insertFixup(z);
    }

    private Node insertFixup(Node z) {
        Node y = new Node();

        while (z.parent.color == Color.RED) {
            if (z.parent == z.parent.parent.left) {
                y = z.parent.parent.right;
                if (y.color == Color.RED) {
                    //Case 1
                    z.parent.color = Color.BLACK;
                    y.color = Color.BLACK;
                    z.parent.parent.color = Color.RED;
                    z = z.parent.parent;
                } else {
                    //Case 2
                    if (z == z.parent.right) {
                        z = z.parent;
                        leftRotate(z);
                    }
                    //Case 3
                    z.parent.color = Color.BLACK;
                    z.parent.parent.color = Color.RED;
                    rightRotate(z.parent.parent);
                }
            } else {
                y = z.parent.parent.left;
                if (y.color == Color.RED) {
                    //Case 1
                    z.parent.color = Color.BLACK;
                    y.color = Color.BLACK;
                    z.parent.parent.color = Color.RED;
                    z = z.parent.parent;
                } else {
                    //Case 2
                    if (z == z.parent.left) {
                        z = z.parent;
                        rightRotate(z);
                    }
                    //Case 3
                    z.parent.color = Color.BLACK;
                    z.parent.parent.color = Color.RED;
                    leftRotate(z.parent.parent);
                }
            }
        }
        root.color = Color.BLACK; 
        return z;
    }

    public void delete(int key) {
        Node search = find(key);
        if (search!=NIL.NilNode())
            delete(search);
    }

    private void delete(Node z) {
        Node y;
        Node x;
        if (z.left == NIL.NilNode() || z.right == NIL.NilNode())
            y = z;
        else
            y = getMin(z.right);

        if (y.left != NIL.NilNode())
            x = y.left;
        else
            x = y.right;
        x.parent = y.parent;
        if (y.parent == NIL.NilNode()) {
            root = x;
        } else {
            if (y.parent.left == y)
                y.parent.left = x;
            else 
                y.parent.right = x;
        }
        if (y != z) {
            z.key = y.key;
        }
        if (y.color == Color.BLACK) {
            deleteFixup(x);
        }
    }

    private void deleteFixup(Node x) {
        Node w;
        while (x != root && x.color == Color.BLACK) {
            if (x == x.parent.left) {
                w = x.parent.right;
                if (w.color == Color.RED) {
                    w.color = Color.BLACK;
                    x.parent.color = Color.RED;
                    leftRotate(x.parent);
                    w = x.parent.right;
                }
                if (w.left.color == Color.BLACK && w.right.color == Color.BLACK) {
                    w.color = Color.RED;
                    x = x.parent;
                } 
                else {
                    if (w.right.color == Color.BLACK) {
                        w.left.color = Color.BLACK;
                        w.color = Color.RED;
                        rightRotate(w);
                        w = x.parent.right;
                    }
                    w.color = x.parent.color;
                    x.parent.color = Color.BLACK;
                    w.right.color = Color.BLACK;
                    leftRotate(x.parent);
                    x = root;
                }
            } else {
                w = x.parent.left;
                if (w.color == Color.RED) {
                    w.color = Color.BLACK;
                    x.parent.color = Color.RED;
                    rightRotate(x.parent);
                    w = x.parent.left;
                }
                if (w.left.color == Color.BLACK && w.right.color == Color.BLACK) {
                    w.color = Color.RED;
                    x = x.parent;
                } else {
                    if (w.left.color == Color.BLACK) {
                        w.right.color = Color.BLACK;
                        w.color = Color.RED;
                        leftRotate(w);
                        w = x.parent.left;
                    }
                    w.color = x.parent.color;
                    x.parent.color = Color.BLACK;
                    w.left.color = Color.BLACK;
                    rightRotate(x.parent);
                    x = root;
                }
            }

        }
        x.color = Color.BLACK;
    }

    private Node getMin(Node x) {
        while (x.left != NIL.NilNode()) {
            x = x.left;
        }
        return x;
    }

    private Node getMax(Node x) {
        while (x.right != NIL.NilNode()) {
            x = x.right;
        }
        return x;
    }

    public Node find(int key) {
        return find(root, key);
    }

    private Node find(Node x, int key) {
        if (x == null || x == NIL.NilNode() || key == x.key) {
            return x;
        }
        if (key < x.key){
            return find(x.left, key);
        } else {
            return find(x.right, key);
        }
    }


    //===================PRINT===========================
    public void print() {
        if (root==null || root == NIL.NilNode()) {
            WriteLine("Tree is empty");
            return;
        }
        NodeList[] levels = getTreePerLevels();

        int height = getHeight(this.root);
        for (int i=height+1;i>=0;i--) {
            int padding = formula(i, this.digits);
            int num = height+1-i;
            for (int j=0;j<Math.Pow(2, num);j++) {
                if (levels[num][j]!=null) {
                    if (levels[num][j]!=NIL.NilNode())
                        encapsulate(padding, j, levels[num][j].key, this.digits, levels[num][j].color);
                    else    
                        encapsulate(padding, j, int.MinValue, this.digits, Color.BLACK);
                } else {
                    encapsulate(padding, j, int.MinValue, this.digits, Color.Blue);    
                }
            }
            WriteLine();
        }
    }

    public int Height {
        get {
            return getHeight(this.root);
        }
    }

    private static int getHeight(Node n) {
        if (n==null || n==NIL.NilNode()) {
            return -1;
        }
        return 1+Max(getHeight(n.left), getHeight(n.right));
    }


    private int formula(int i, int digits) {
        return (int)((digits*Math.Pow(2,i)+digits*(Math.Pow(2,i)-1))/2-(digits/2));
    }

    static void encapsulate(int padding, int count, int keyue, int digits, Color c) {
        if (count==0) {
            if (padding!=0) {
                if (keyue!=int.MinValue) {
                    for (int i=1;i<=padding/2;i++) {
                        Write(" ");
                    }
                    for (int i=padding/2+1;i<=padding;i++) {
                        Write("-");
                    }
                } else {
                    for (int i=1;i<=padding;i++) {
                        Write(" ");
                    }
                }
            }
            if (c==Color.BLACK) {
                Console.BackgroundColor = ConsoleColor.Black;
            } else if (c==Color.RED) {
                Console.BackgroundColor = ConsoleColor.Red;
            } else {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            if (keyue==int.MinValue) {
                for (int i=0;i<digits;i++) {
                    Write(" ");
                }
            } else{
                Write(keyue.ToString("D"+digits.ToString()));
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            if (keyue!=int.MinValue) {
                for (int i=1;i<=((padding+digits/2)/2);i++) {
                    Write("-");
                }
                for (int i=(padding+digits/2)/2;i<padding+digits/2;i++) {
                    Write(" ");
                }
            } else {
                for (int i=1;i<=padding+digits/2;i++) {
                    Write(" ");
                }
            }
        } else {
            if (keyue!=int.MinValue) {
                for (int i=1;i<=(padding+digits/2)/2+1;i++) {
                    Write(" ");
                }
                for (int i=(padding+digits/2)/2+2;i<=padding+digits/2;i++) {
                    Write("-");
                }
            } else {
                for (int i=1;i<=padding+digits/2;i++) {
                    Write(" ");
                }
            }
            if (c==Color.BLACK) {
                Console.BackgroundColor = ConsoleColor.Black;
            } else if (c==Color.RED) {
                Console.BackgroundColor = ConsoleColor.Red;
            } else {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            if (keyue==int.MinValue) {
                for (int i=0;i<digits;i++) {
                    Write(" ");
                }
            } else{
                Write(keyue.ToString("D"+digits.ToString()));
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            if (keyue!=int.MinValue) {
                for (int i=1;i<=(padding+digits/2)/2;i++) {
                    Write("-");
                }
                for (int i=(padding+digits/2)/2+1;i<=padding+digits/2;i++) {
                    Write(" ");
                }
            } else {
                for (int i=1;i<=padding+digits/2;i++) {
                    Write(" ");
                }
            }
        }
    }

    private NodeList[] getTreePerLevels() {
        int height = getHeight(this.root);
        NodeList[] levels = new NodeList[height+2];
        for (int i=0;i<levels.Length;i++) {
            levels[i] = new NodeList((int)Math.Pow(2, i));
        }
        populate(ref levels, this.root, 0);
        return levels;
    }

    static void populate(ref NodeList[] levels, Node n, int level) {
        if (n==null) {
            levels[level].add(null);
            int j = 1;
            for (int i=level+1;i<levels.Length;i++) {
                for (int k=1;k<=j;k++){
                    levels[i].add(null);
                    levels[i].add(null);
                }
                j++;
            }
            return;
        }
        if (n==NIL.NilNode()) {
            levels[level].add(NIL.NilNode());
            int j = 1;
            for (int i=level+1;i<levels.Length;i++) {
                for (int k=1;k<=j;k++){
                    levels[i].add(null);
                    levels[i].add(null);
                }
                j++;
            }
            return;   
        }
        populate(ref levels, n.left, level+1);
        levels[level].add(n);
        populate(ref levels, n.right, level+1);
    }
}

class Prog {

    static void insert(ref RedBlackTree t, string s) {
        if (s=="") { return; }
        string[] a = s.Split();
        int[] integers = new int[a.Length];
        for (int i=0;i<a.Length;i++) {
            integers[i] = int.Parse(a[i]);
        }
        foreach (int i in integers) {
            t.insert(i);
        }
    }

    static void delete(ref RedBlackTree t, string s) {
        if (s=="") { return; }
        string[] a = s.Split();
        int[] integers = new int[a.Length];
        for (int i=0;i<a.Length;i++) {
            integers[i] = int.Parse(a[i]);
        }
        foreach (int i in integers) {
            t.delete(i);
            t.print();
            ReadLine();
        }
    }

    static void find(ref RedBlackTree t, string s) {
        if (s=="") { return; }
        string[] a = s.Split();
        int[] integers = new int[a.Length];
        for (int i=0;i<a.Length;i++) {
            integers[i] = int.Parse(a[i]);
        }
        foreach (int i in integers) {
            if (t.find(i)!=null) {
                WriteLine(i+": Present");
            } else {
                WriteLine(i+": Absent");
            }
        }
        ReadLine();
    }

    static void test() {
        // ============ RANDOM ORDER =============
        WriteLine("Testing both trees inserting numbers in random order");
        WriteLine("Red Black Tree:");
        
        // ============ RED BLACK TEST ==========
        RedBlackTree rbt = new RedBlackTree();
        Random r = new Random();
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        List<int> rblist = new List<int>();
        for (int i=0;i<50000;i++) {
            int n = r.Next(1000000);
            rblist.Insert(i, n);
            rbt.insert(n);
        }
        foreach (int i in rblist) {
            rbt.delete(i);
        }
        rbt.print();
        stopwatch.Stop();
        WriteLine("Time elapsed: {0} milliseconds", stopwatch.Elapsed.TotalMilliseconds);
        WriteLine();

        // ======== BINARY TEST ===================
        WriteLine("Binary Tree:");
        BinaryTree bt = new BinaryTree();
        Stopwatch stopwatch2 = new Stopwatch();
        stopwatch2.Start();
        List<int> blist = new List<int>();
        for (int i=0;i<50000;i++) {
            int n = r.Next(1000000);
            blist.Insert(i, n);
            bt.insert(n);
        }
        foreach (int i in blist) {
            bt.delete(i);
        }
        bt.print();
        stopwatch2.Stop();
        WriteLine("Time elapsed: {0} milliseconds", stopwatch2.Elapsed.TotalMilliseconds);
        WriteLine();

        // ================== ASCENDING ORDER ===========
        WriteLine("Testing both trees inserting numbers in ascending order");
        WriteLine("Red Black Tree:");
        /*List<int> order = new List<int>();
        for (int i=0;i<1000000;i++) {

        }*/

        // ============ RED BLACK TEST ==========
        rblist.Sort();
        stopwatch.Reset();
        stopwatch.Start();
        foreach (int n in rblist) {
            rbt.insert(n);
        }
        foreach (int n in rblist) {
            rbt.delete(n);
        }
        rbt.print();
        stopwatch.Stop();
        WriteLine("Time elapsed: {0} milliseconds", stopwatch.Elapsed.TotalMilliseconds);
        WriteLine();

        // ======== BINARY TEST ===================
        WriteLine("Binary Tree");
        blist.Sort();
        stopwatch2.Reset();
        stopwatch2.Start();
        foreach (int n in blist) {
            bt.insert(n);
            /*bt.print();*/
        }
        foreach (int n in blist) {
            bt.delete(n);
        }
        bt.print();
        stopwatch2.Stop();
        WriteLine("Time elapsed: {0} milliseconds", stopwatch2.Elapsed.TotalMilliseconds);


    }



    static void Main() {
        RedBlackTree t = new RedBlackTree();
        while (true) {
            Console.Clear();
            WriteLine("Enter command: \ni: insert integers \nd: delete integers \nf: find integers \np: print \nt: test \nq: quit");
            char command = ReadKey().KeyChar;
            switch (command) {
                case 'i':
                Console.Clear();
                Write("Enter integers: ");
                insert(ref t, ReadLine());
                break;

                case 'd':
                Console.Clear();
                Write("Enter integers: ");
                delete(ref t, ReadLine());
                break;

                case 'f':
                Console.Clear();
                Write("Enter integers: ");
                find(ref t, ReadLine());
                WriteLine("Press ENTER to continue...");
                ReadLine();
                break;

                case 'p':
                Console.Clear();
                t.print();
                WriteLine("Press ENTER to continue...");
                ReadLine();
                break;

                case 't':
                Console.Clear();
                test();
                WriteLine("Press ENTER to continue...");
                ReadLine();
                break;

                case 'q':
                return;
                break;

                default:
                Console.Clear();
                WriteLine("Wrong command");
                WriteLine("Press ENTER to continue...");
                ReadLine();
                break;
            }
        }
    }
}