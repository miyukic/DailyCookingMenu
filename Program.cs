/*
*  dotnet run //debug
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Yk.DailyCookingMenu
{

public enum TimeZoneTAG
{
    All,
    Breakfast,
    Launch,
    Dinner,
    Length
}

// ご飯の中の一つの品をあらわすクラス
//
public interface ICuisine
{
    string Name { get; set; }
    HashSet<TimeZoneTAG> TimeZone { get; set; }
    string RecipeURL { get; set; }
    HashSet<string> CategorySet { get; }
}

public class Cuisine : ICuisine
{
    public string Name { get; set; }
    public HashSet<TimeZoneTAG> TimeZone { get; set; }
    public string RecipeURL { get; set; }
    public HashSet<string> CategorySet { get; }

    public Cuisine(string name = "none", HashSet<TimeZoneTAG> timeZone = null, string recipe = "", HashSet<string> categorySet = null)
    {
    this.Name = name;
    if (timeZone == null) {
        this.TimeZone = new HashSet<TimeZoneTAG>();
        this.TimeZone.Add(TimeZoneTAG.All);
    }
    this.TimeZone = timeZone;
    this.RecipeURL = recipe;
    if (categorySet != null) {
        this.CategorySet = categorySet;
    } else {
        this.CategorySet = new HashSet<string>();
    }
    }

    public Cuisine AddTimeZone(TimeZoneTAG timeZone)
    {
        TimeZone.Add(timeZone);
        return this;
    }

    public Cuisine RemoveTimeZone(TimeZoneTAG timeZone)
    {
        TimeZone.Remove(timeZone);
        return this;
    }

    public Cuisine AddCategory(String category)
    {
        if (Data.CategoryDict.ContainsKey(category)) {
            int num = Data.CategoryDict[category];
            Data.CategoryDict[category] = num + 2;
        } else {
            Data.CategoryDict.Add(category, 1);
        }
        CategorySet.Add(category);
        return this;
    }

    public Cuisine RemoveCategory(String category)
    {
        if (Data.CategoryDict.ContainsKey(category)) {
            int num = Data.CategoryDict[category];
            if (num == 1) {
                Data.CategoryDict.Remove(category);
            } else {
                Data.CategoryDict[category] = num - 1;
            }
        }
        CategorySet.Remove(category);
        return this;
    }

    public override int GetHashCode()
    {
        return this.Name.GetHashCode() ^ this.TimeZone.GetHashCode()
        ^ this.RecipeURL.GetHashCode() ^ this.CategorySet.GetHashCode();
    }

    public override bool Equals(Object other)
    {
        if (other == null || GetType() != other.GetType())
        {
            return false;
        }
        var otherCuisine = (Cuisine)other;
        bool b = true;
        b &= otherCuisine.Name      == this.Name;
        b &= otherCuisine.TimeZone  == this.TimeZone;
        b &= otherCuisine.RecipeURL == this.RecipeURL;
        b &= otherCuisine.CategorySet.SetEquals(otherCuisine.CategorySet);
        return b;
    }
}

public class Dish : Cuisine
{

    public Dish(string name = "none", HashSet<TimeZoneTAG> timeZone = null, string recipe = "", HashSet<string> categorySet = null) :
    base(name, timeZone,recipe, categorySet) { }

    public override bool Equals(object obj)
    {

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return base.Equals (obj);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        throw new System.NotImplementedException();
        return base.GetHashCode();
    }

}

public static class Data
{
    // 献立のメニューに使われる料理の一覧を保持する
    public static IList<Dish> DishList { get; set; } = new List<Dish>();

    // 料理に付けられるすべてのカテゴリーとそれぞれのカテゴリーの数を保持・管理している
    public static IDictionary<string, int> CategoryDict {get; set; } = new Dictionary<string, int>();
}

class Program
{
    private static HashSet<TimeZoneTAG> MakeTimeZone(TimeZoneTAG[] tags)
    {
        var tagSet = new HashSet<TimeZoneTAG>();
        foreach (var item in tags)
        {
            tagSet.Add(item);
        }
            return tagSet;
    }

    public static void FizzBuzz(int[] nums)
    {
        var len = nums.Length;
        for (var i = 0; i < len; i++) {
            if (nums[i] % 5 == 0 && nums[i] % 3 == 0) {
                Console.WriteLine("Fizz");
            } else if (nums[i] % 5 == 0) {
                Console.WriteLine("Buzz");
            } else if (nums[i] % 3 == 0) {
                Console.WriteLine("FizzBuzz");
            }
        }
    }

    public static void Main(string[] args)
    {
        GetAPI().AddDish(new Dish(
            name: "カレー",
            timeZone: MakeTimeZone(new TimeZoneTAG[2] {TimeZoneTAG.All, TimeZoneTAG.Breakfast})
            )).AddCategory("メイン");
        GetAPI().AddDish(new Dish("ラーメン")).AddCategory("メイン");
        GetAPI().AddDish(new Dish("オムライス")).AddCategory("メイン");
        GetAPI().AddDish(new Dish("蕎麦")).AddCategory("メイン").AddCategory("麺類");
        GetAPI().AddDish(new Dish("チャーハン")).AddCategory("主食").AddCategory("ごはん");
        GetAPI().AddDish(new Dish("うどん")).AddCategory("メイン");
        GetAPI().AddDish(new Dish("棒棒鶏")).AddCategory("サラダ").AddCategory("副菜");
        GetAPI().AddDish(new Dish("味噌汁")).AddCategory("汁物");
        GetAPI().AddDish(new Dish("けんちん汁")).AddCategory("汁物");
        GetAPI().AddDish(new Dish("和風スパゲティ")).AddCategory("メイン").AddCategory("麺類");
        GetAPI().AddDish(new Dish("ハンバーグ")).AddCategory("メイン").AddCategory("主菜");
        GetAPI().AddDish(new Dish("ドライカレー")).AddCategory("メイン").AddCategory("ごはんもの");
        GetAPI().AddDish(new Dish("ヒレカツ")).AddCategory("主菜");
        GetAPI().AddDish(new Dish("ちゃんちゃん焼き")).AddCategory("メイン").AddCategory("主菜").AddCategory("おかず");
        CookingRobot cookingRobot = new CookingRobot();
        int 食事の品数 = 4;
        var meal = cookingRobot.PlanMeal(Yk.DailyCookingMenu.TimeZoneTAG.All, 食事の品数);
        meal.ForEach(dish => Console.WriteLine(dish.Name));

    }

    public static API GetAPI()
    {
        return DcmAPI.GetInstance();
    }
}

// 他のプログラムに公開するAPI
public interface API
{
    //internal static API Api { get; }
    public Dish AddDish(Dish dish);
    public abstract List<Dish> GetDishList();
}

sealed class DcmAPI : API
{
    // internal static API Api { get; } = new DcmAPI();
    private static API api = new DcmAPI();
    private DcmAPI(){}

    public static API GetInstance() {
        return DcmAPI.api;
    }

    public Dish AddDish(Dish dish)
    {
        if (dish != null) {
            Data.DishList.Add(dish);
            return dish;
        } else {
            return null;
        }
    }

    public List<Dish> GetDishList()
    {
        return null;
    }
}

// 献立の構成を表す
public class MealConstitution
{
    public ICollection<MealConstitutionComponent> Constitution { get; } = new List<MealConstitutionComponent>();

    public MealConstitution() : this(null) { }

    public MealConstitution(ICollection<MealConstitutionComponent> constitution)
    {
        if (constitution == null) {
            return;
        }
        this.Constitution = constitution;
    }

    public MealConstitution AddMealComponent(MealConstitutionComponent component)
    {
        Constitution.Add(component);
        return this;
    }
}

public enum SearchMode
{
    Default = 0,
    ExacrtMatch = 1,
    PartialMatch = 2,
    NotMatch = 3,
    Ignore = 4,
    Length = 5
}

// 献立の構成要素(料理)
public class MealConstitutionComponent : Cuisine
{
    // 各プロパティのSearchModeの関連付けを保持
    private Dictionary<string, SearchMode> searchModeDict = new Dictionary<string, SearchMode>();
    private Type type = typeof(ICuisine);
    private readonly string[] parmitedPropaty = { "RecipeURL" };
    public const SearchMode DEFAULT_SEARCHMODE = SearchMode.ExacrtMatch;

    public MealConstitutionComponent() : base()
    {
        foreach(var i in this.type.GetProperties()) {
            searchModeDict.Add(i.Name, SearchMode.Default);
        }
    }

    // サーチモードの影響のあるプロパティ一覧
    // 現在はコレクションプロパティのみ
    internal bool IsAbleToSetSearchMode(string propaty)
    {
        if (this.parmitedPropaty.Contains(propaty)) return true;
        foreach(var i in this.type.GetProperties()) {
            if (i.Name == propaty) {
                if (i.PropertyType.GetInterface(nameof(IEnumerable<string>)) != null) {
                    return true;
                } else {
                    return false;
                }
            }
        }
        return false;
    }

    public string[] GetPropaties()
    {
        string[] propaties = new string[searchModeDict.Keys.Count];
        searchModeDict.Keys.CopyTo(propaties, 0);
        return propaties;
    }

    public void PropatyにSearchModeを設定(string propatyName, SearchMode mode)
    {
        if (searchModeDict.ContainsKey(propatyName)) {
            searchModeDict[propatyName] = mode;
        } else {
            throw new DoNotFoundPropatyException();
        }
    }

    public SearchMode GetPropatySearchMode(string propatyName)
    {
        if (searchModeDict.ContainsKey(propatyName)) {
                SearchMode mode = searchModeDict[propatyName];
                return mode;
        } else {
            throw new DoNotFoundPropatyException();
        }
    }

    // Dishオブジェクトと比較し、条件と合致するか判断する true->合致 false->合致しない
    public bool EqualTo(Dish dish)
    {
        if (dish == null) return false;
        bool result = true;
        result &= dish.Name == this.Name;
        var catSearchmode = GetPropatySearchMode(nameof(this.CategorySet));
        if ((catSearchmode == SearchMode.Default && DEFAULT_SEARCHMODE == SearchMode.PartialMatch) || catSearchmode == SearchMode.PartialMatch) {
            result &= dish.CategorySet.SetEquals(this.CategorySet) || dish.CategorySet.Overlaps(this.CategorySet);
        } else if ((catSearchmode == SearchMode.Default && DEFAULT_SEARCHMODE == SearchMode.NotMatch) || catSearchmode == SearchMode.NotMatch) {
            foreach (var item in this.CategorySet) {
                if (dish.CategorySet.Contains(item)) {
                    result &= false;
                    break;
                }
            }
        } else if ((catSearchmode == SearchMode.Default && DEFAULT_SEARCHMODE == SearchMode.ExacrtMatch) || catSearchmode == SearchMode.ExacrtMatch) {
            result &= dish.CategorySet.SetEquals(this.CategorySet);
        } else if ((catSearchmode == SearchMode.Default && DEFAULT_SEARCHMODE == SearchMode.Ignore) || catSearchmode == SearchMode.Ignore) {
            result &= true;
        }
        var tzSearchmode = GetPropatySearchMode(nameof(this.TimeZone));
        if ((tzSearchmode == SearchMode.Default && DEFAULT_SEARCHMODE == SearchMode.Ignore) || tzSearchmode == SearchMode.Ignore) {
            result &= true;
        } else {
            result &= this.RecipeURL == dish.RecipeURL;
        }
        return result;
    }

}

class DoNotFoundPropatyException : Exception
{
    public DoNotFoundPropatyException() : this(
        "You specified a property that does not exist. Please check the property name and try it. 存在しないプロパティを指定しました。プロパティ名を確認してください。"
        ) {}
    public DoNotFoundPropatyException(string message) : base(message) {}
}

// ご飯の献立を考えるロボットクラス
public class CookingRobot
{
    public string RobotName { get; set; } = "MiyukiCookingRobot";

    // 時間帯(TimeZone)と品数から献立を作成する
    public List<Dish> PlanMeal(Yk.DailyCookingMenu.TimeZoneTAG timeZone, int quantity)
    {
        int len = Data.DishList.Count;
        List<int> randomNumberList = new List<int>();
        var resultMeal = new List<Dish>();

        if (quantity < len && 1 < len) {
            randomNumberList = GenerateRandomNumberList(len, quantity);
        } else {
            Console.WriteLine("品数に対してDishの数が足りませんでした。");
            return resultMeal;
        }
        //Console.WriteLine("randomumberList => ");
        //randomumberList.ForEach( i => Console.Write(i + ","));
        //Console.Write("\n");
        randomNumberList.ForEach(randomNum => resultMeal.Add(Data.DishList[randomNum]));
        return resultMeal;
    }

    // 献立の構成（MealConstitution）から献立を作成する
    public HashSet<Dish> PlanMeal(MealConstitution constitution)
    {
        var resultMeal = new HashSet<Dish>();
        var allDishList = Data.DishList;
        var tempList = new List<List<Dish>>();
        foreach (MealConstitutionComponent component in constitution.Constitution)
        {
            var t = SearchDishFromMealConstitutionComponent(component)
            tempList.Add(t);
)           // var list = SearchDishFrokmMealConstitutionComponent(component);
            // Dish dish = null;
            // do {
            //     var pointer = 0;
            //     dish = list[GenerateRandomNumberList(list.Count(), list.Count())[pointer]];
            //     if (pointer++ > (list.Count() - 1)) {
            //         // Todo: 料理の献立が他の条件のものと被って、他に選択肢がない場合どうするか？
            //         // 被った場合は被ったものをこっちに渡して、
            //         //resultMeal.Contains(dish);
            //         break;
            //     }
            // } while (!resultMeal.Add(dish));
        }
        return resultMeal;
    }

    //
    public List<Dish> SearchDishFromMealConstitutionComponent(MealConstitutionComponent component)
    {
        var resultList = new List<Dish>();
        if (component == null) return resultList;
        foreach (var item in Data.DishList)
        {
            var ans = component.EqualTo(item);
            if (!ans) continue;
            resultList.Add(item);
        }
        return resultList;
    }

    public List<int> GenerateRandomNumberList(int volume, int quantity)
    {
        if (quantity < 1 || volume < 1 || volume < quantity) return null;
        var コピー元の数字の配列 = new List<int>();
        for (var i = 0; i < volume; i++) {
            コピー元の数字の配列.Insert(i, i);
        }
        var rnd = new Random();
        var resultList = new List<int>();
        for (var i = 0; i < quantity; i++) {
            var t = rnd.Next(0, コピー元の数字の配列.Count);
            resultList.Add(コピー元の数字の配列[t]);
            コピー元の数字の配列.RemoveAt(t);
        }
        return resultList;
    }
}

} // namespace DailyCookingMenu
