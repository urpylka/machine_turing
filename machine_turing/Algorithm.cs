using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace machine_turing
{
    public class Lenta
    {
        int CONST_MAX_LENTA = 0;
        private Char[] lenta;
        public Char getChar(int pos)
        {
            return lenta[pos];
        }
        public void setChar(Char ch, int pos)
        {
            lenta[pos] = ch;
        }
        int cur = 0;
        public int getCur()
        {
            return cur;
        }
        public void incCur()
        {
            cur++;
            if (cur == 200) throw new Exception("Индекс ленты превышен 200");
        }
        public void decCur()
        {
            cur--;
            if (cur == 0) throw new Exception("Индекс ленты превышен 0");
        }
        //public Lenta(int CONST_MAX_LENTA = 200, Char ch_null = '_')
        //{
        //    this.CONST_MAX_LENTA = CONST_MAX_LENTA;
        //    lenta = new Char[this.CONST_MAX_LENTA];
        //    for (int i = 0; i < this.CONST_MAX_LENTA; i++) setChar(ch_null, i);
        //}
        public Lenta(String[] lenta,Alphabet alp)
        {
            this.CONST_MAX_LENTA = lenta.Length;
            this.lenta = new Char[this.CONST_MAX_LENTA];
            for (int i = 0; i < this.CONST_MAX_LENTA; i++)
            {
                if(alp.inAlphabet(lenta[i][0])) this.lenta[i] = lenta[i][0];
                else throw new Exception("Символ \""+ lenta[i][0] + "\"не принадлежит алфавиту");
            }
        }
        public Char getPrev()
        {
            if (cur - 1 < 0) throw new Exception("Выход за границу ленты");
            else
            {
                cur--;
                return getChar(cur);
            }
        }
        public Char getCurrent()
        {
            return getChar(cur);
        }
        public Char getNext()
        {
            if (cur + 1 >= CONST_MAX_LENTA) throw new Exception("Выход за границу ленты");
            else
            {
                cur++;
                return getChar(cur);
            }
        }
        public void setCurrent(Char ch)
        {
            setChar(ch, cur);
        }
        public String[,] toString()
        {
            String[,] buffer = new String[2, CONST_MAX_LENTA];
            for (int i = 0; i < CONST_MAX_LENTA; i++)
            {
                buffer[0, i] = i.ToString();
                buffer[1, i] = getChar(i).ToString();
            }
            return buffer;
        }
    }
    public class Alphabet
    {
        public Char[] alphabet;
        //public Alphabet(int length = 3)
        //{
        //    if(length==3) alphabet = new Char[3] { '|', '*', '_' };
        //    else throw new Exception("Начальный алфавит может быть только трёх-символьным.");
        //}
        public Alphabet(String[] frMatr)
        {
            int length = frMatr.Length;
            alphabet = new Char[length];
            for (int i = 0; i < length; i++)
            {
                Char buf = frMatr[i][0];
                if (consistsAllAlphabet(buf))
                {
                    if (!inAlphabet(buf)) alphabet[i] = buf; //только первый символ в строке
                    else throw new Exception("Символ повторяется: " + buf);
                }
                else throw new Exception("Символ не пренадлежит множеству алфавита.");
            }
        }
        public static bool consistsAllAlphabet(Char ch)
        {
            Char[] allAlphabet = new Char[66] { '_', '#', '*', '§', '\\', '/', '|', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я' };
            bool flag = false;
            foreach (Char ch2 in allAlphabet) if (ch2 == ch) flag = true;
            return flag;
        }
        public bool inAlphabet(Char ch)
        {
            bool flag = false;
            foreach (Char ch2 in alphabet) if (ch2 == ch) flag = true;
            return flag;
        }
        public int findPosition(Char ch)
        {
            for (int i = 0; i < alphabet.Length; i++) if (alphabet[i] == ch) return i;
            throw new Exception("Выбранный символ \"" + ch + "\" не найден в алфавите.");
            //return -1;
        }
        public int addSymbol(Char ch)
        {
            if (consistsAllAlphabet(ch))
            {
                if (inAlphabet(ch)) throw new Exception("Введенный символ \"" + ch + "\" уже используется в алфавите.");
                else
                {
                    Char[] buffer = alphabet;
                    alphabet = new Char[buffer.Length+1];
                    alphabet = buffer;
                    alphabet[buffer.Length] = ch;
                    return buffer.Length;
                }
            }
            else throw new Exception("Введенный символ \"" + ch + "\" не может использоваться в алфавите.");
        }
        public int deleteSymbol(Char ch, Algorithm algo)
        {
            if (algo.consistsSymbol(ch)) throw new Exception("Удаляемый символ \"" + ch + "\" используется в алгоритме, для его удаления сначала удалите ячейки что на него ссылаются.");
            else
            {
                if (!inAlphabet(ch)) throw new Exception("Выбранный символ \"" + ch + "\" не используется в алфавите.");
                else
                {
                    //ДОЛЖНА БЫТЬ ПРОВЕРКА НА ТО ЧТО СИМВОЛ НЕ ИСПОЛЬЗУЕТСЯ В АЛГОРИТМЕ
                    int position = findPosition(ch);
                    Char[] buffer = alphabet;
                    alphabet = new Char[buffer.Length - 1];
                    int i = 0;
                    for (; i < position; i++) alphabet[i] = buffer[i];
                    i++; //проходим удаляемый элемент
                    for (; i < buffer.Length; i++) alphabet[i - 1] = buffer[i];
                    return position;
                }
            }
        }
    }
    public struct States
    {
        public int[] states;
        //public States(int length = 1)
        //{
        //    if (length == 1) states = new int[1] { 1 };
        //    else throw new Exception("Количество состояний вначале всегда равно одному.");
        //}
        public States(String[] states)
        {
            this.states = new int[states.Length];
            for (int i = 0; i < states.Length; i++)
            {
                if (states.Length <= 200)
                {
                    if ((states[i] == null) || (states[i] == "")) throw new Exception("Не задан номер состояния. Введите его номер!");
                    try
                    {
                        if (!inStates(Int32.Parse(states[i])))
                        {
                            this.states[i] = Int32.Parse(states[i]);
                            if (this.states[i] > 200) throw new Exception("Номер состояния слишком большой, введите число от 1 до 200. Ваш: " + this.states[i]);
                        }
                        else throw new Exception("Такой номер состояния уже есть: " + states[i]);
                    }
                    catch (Exception ex) { throw new Exception("Вы не корректно ввели номер состояния: " + states[i]+"\n"+ex.Message); }
                }
                else throw new Exception("Превышено количество состояний");
            }
        }
        public bool inStates(int state)
        {
            bool flag = false;
            foreach (int st in states) if (st == state) flag = true;
            if (!flag && state == 0) flag = true; //состояние выхода существует, иначе при добавлении ячейки с переходом в нулевое состояние будет ошибка
            return flag;
        }
        public int findPosition(int st)
        {
            for (int i = 0; i < states.Length; i++) if (states[i] == st) return i;
            throw new Exception("Выбранное состояние \"" + st + "\" не найдено.");
            //return -1;
        }
        public int addState(int state)
        {
            return addStateAt(state, states.Length);
        }
        public int addStateAt(int state, int position)
        {
            if (position + 1 <= states.Length)
            {
                if (inStates(state)) throw new Exception("Это состояние уже существует.");
                else if ((state <= 200) && (state > 1))
                {
                    int[] buffer = states;
                    states = new int[buffer.Length + 1];
                    int i = 0;
                    for (; i < position; i++) states[i] = buffer[i];
                    i++; //проходим удаляемый элемент
                    states[i] = state;
                    for (; i < buffer.Length; i++) states[i] = buffer[i - 1];
                    return position;
                }
                else throw new Exception("Введенное \"" + state + "\" состояние выходит за границы.");
            }
            else throw new Exception("Введенное \"" + position + "\" позиция выходит за границы массива.");
        }
        public int deleteState(int state, Algorithm algo)
        {
            if (algo.consistsState(state)) throw new Exception("Удаляемое состояние \"" + state + "\" используется в алгоритме, для его удаления сначала удалите ячейки что на него ссылаются.");
            else
            {
                if (!inStates(state)) throw new Exception("Удаляемое состояние \"" + state + "\" не создано.");
                else
                {
                    int position = findPosition(state);
                    int[] buffer = states;
                    states = new int[buffer.Length - 1];
                    int i = 0;
                    for (; i < position; i++) states[i] = buffer[i];
                    i++; //проходим удаляемый элемент
                    for (; i < buffer.Length; i++) states[i - 1] = buffer[i];
                    return position;
                }
            }
        }
    }
    public class Cell
    {
        private Char symbol;
        public Char getSymbol()
        {
            if (symbol == '\0') symbol = '_';
            return symbol;
        }
        private int nextState;
        public int getNextState()
        {
            return nextState;
        }
        private Char route;
        public Char getRoute()
        { return route; }
        public bool isRoute(Char ch)
        {
            switch(ch)
            {
                case 'r':
                    break;
                case 'l':
                    break;
                case 'n':
                    break;
                default:
                    return false;
            }
            return true;
        }
        public Cell(String cell, Alphabet alpha, States state)
        {
            if (cell.Length >= 3)
            {
                try
                {
                    Char symbol = cell[0];
                    int nextState = System.Convert.ToInt32(cell.Substring(1, cell.Length - 2));
                    Char route = (Char)cell[cell.Length - 1];
                    //=======================================
                    if (!alpha.inAlphabet(symbol)) throw new Exception("В алфавите нет символа \"" + symbol + "\".");
                    else this.symbol = symbol;
                    if (!state.inStates(nextState)) throw new Exception("Пока не существует состояния \"" + nextState + "\".");
                    else this.nextState = nextState;
                    if (!isRoute(route)) throw new Exception("Не существует действия \"" + route + "\".");
                    else this.route = route;
                }
                catch (Exception ex) { throw new Exception("Введенная строка не соотвествует формату, из-за чего возникает ошибка приведения типов \"" + cell + "\".\n" + ex.Message); }
            }
            else
            {
                if(cell.Length>=1) throw new Exception("Введенная строка слишком короткая \"" + cell + "\".");
            }
        }
        public String toString()
        {
            return ""+this.symbol+this.nextState+this.route;
        }
    }
    public class Algorithm
    {
        private Cell[,] matrix;
        private Alphabet alphabet;
        private States states;
        private Lenta lenta;
        public bool consistsState(int st)
        {
            for (int i = 1; i < matrix.GetLength(0); i++)
                for (int j = 1; j < matrix.GetLength(1); j++)
                    if (st == matrix[i, j].getNextState()) return true;
            return false;
        }
        public bool consistsSymbol(Char ch)
        {
            for (int i = 1; i < matrix.GetLength(0); i++)
                for (int j = 1; j < matrix.GetLength(1); j++)
                    if (ch == matrix[i, j].getSymbol()) return true;
            return false;
        }

        public Algorithm(String[,] lenta, String[,] algo)
        {

            String[] buffer_alphabet = new String[algo.GetLength(0)-1];
            for (int i = 1; i < algo.GetLength(0); i++) buffer_alphabet[i - 1] = algo[i, 0];
            this.alphabet = new Alphabet(buffer_alphabet);

            String[] buffer_states = new String[algo.GetLength(1)-1];
            for (int i = 1; i < algo.GetLength(1); i++) buffer_states[i - 1] = algo[0, i];
            this.states = new States(buffer_states);

            matrix = new Cell[algo.GetLength(0) - 1, algo.GetLength(1) - 1];
            for (int i = 1; i < algo.GetLength(1); i++)
                for (int j = 1; j < algo.GetLength(0); j++) matrix[j - 1, i - 1] = new Cell(algo[j, i],this.alphabet,this.states);

            String[] buffer_lenta = new String[lenta.GetLength(1)];
            for (int i = 0; i < lenta.GetLength(1); i++) buffer_lenta[i] = lenta[1, i];
            this.lenta = new Lenta(buffer_lenta, alphabet);
        }
        
        int curNumState = 1;
        int lastNumAlpha = 0;
        int lastNumState = 0;
        int lastNumLenta = 0;
        public bool next(DataGridView dgv_lenta, DataGridView dgv_algo) //выполнить следующую операцию
        {
            dgv_algo.Rows[lastNumAlpha].Cells[lastNumState].Style.BackColor = System.Drawing.Color.White;
            dgv_lenta.Rows[1].Cells[lastNumLenta].Style.BackColor = System.Drawing.Color.White;
            //выход из алгоритма как обеспечить?
            if (curNumState != 0)
            {
                int curNumAlpha = alphabet.findPosition(lenta.getCurrent()); //(ищем позицию в алгоритме(берем текущий символ в ленте))
                
                Cell curCell = matrix[curNumAlpha, states.findPosition(curNumState)]; //читаем текущую ячейку

                int f = curNumState + 0 ;
                dgv_algo.Rows[curNumAlpha+1].Cells[states.findPosition(curNumState)+1].Style.BackColor = System.Drawing.Color.LightCyan; //подсветить текущую ячейку
                lastNumAlpha = curNumAlpha + 1;
                lastNumState = states.findPosition(curNumState) + 1;

                lenta.setCurrent(curCell.getSymbol()); //меняю символ в ленте на тот, что в алгоритме
                dgv_lenta.Rows[1].Cells[lenta.getCur()].Value = curCell.getSymbol(); //меняю символ в ленте DGV на тот, что в алгоритме
                //if (curCell.getSymbol() != lenta.getCurrent())
                //{
                //    lenta.setCurrent(curCell.getSymbol()); //меняю символ в ленте на тот, что в алгоритме
                //    dgv_lenta.Rows[1].Cells[lenta.getCur()].Value = curCell.getSymbol(); //меняю символ в ленте DGV на тот, что в алгоритме
                //}
                dgv_lenta.Rows[1].Cells[lenta.getCur()].Style.BackColor = System.Drawing.Color.LightCyan;  //подсветить текущую ячейку в ленте
                lastNumLenta = lenta.getCur();

                curNumState = curCell.getNextState(); //меняю текущее состояние на следующее
                switch (curCell.getRoute()) //меняю индекс в ленте
                {
                    case 'r':
                        lenta.incCur();
                        break;
                    case 'l':
                        lenta.decCur();
                        break;
                    case 'n':
                        break;
                    default:
                        throw new Exception("Переход полученный из текущей ячейки не принадлежить множеству возможных переходов.\nСкорее всего вы попали в неопределенное состояние. Параметры ячейки: " + curCell.toString());
                }
                //ЗДЕСЬ запись ленты с выделением позициии в историю
                return true;
            }
            else return false;
        }
        public void run(DataGridView dgv_lenta, DataGridView dgv_algo) //сюда датагрид и параметры запуска
        {
            int c = 0;
            while(next(dgv_lenta, dgv_algo))
            {
                c++;
                if (c > 1000) break; //ограничение на количество операций (не время)
            }
            if (!next(dgv_lenta, dgv_algo)) throw new Exception("Программа успешно выполнена!");
        }
        public String[,] toString()
        {
            try {
                String[,] buffer = new String[matrix.GetLength(0) + 1, matrix.GetLength(1) + 1];
                for (int i = 1; i < buffer.GetLength(0); i++)
                    buffer[i, 0] = alphabet.alphabet[i-1].ToString();
                for (int i = 1; i < buffer.GetLength(1); i++)
                    buffer[0, i] = states.states[i-1].ToString();
                for (int i = 1; i < buffer.GetLength(0); i++)
                    for (int j = 1; j < buffer.GetLength(1); j++)
                    {
                        buffer[i, j] = matrix[i-1, j-1].toString();
                    }
                return buffer;
            }
            catch (Exception ex) { throw new Exception("Произошла ошибка при рендеренге конечной матрицы: " + ex.Message); }
        }
    }
}
