using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace YeeTech.Infrastructure
{
    public class ValidateUtils
    {
        #region 字段

        //文件名筛选
        private static readonly Regex regex_FileNameFilter =
            new Regex("[<>/\";#$*%]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //图片格式
        private static readonly Regex regex_ImgFormat =
            new Regex(@"\.(gif|jpg|bmp|png|jpeg)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //是否是日期
        private static readonly Regex regex_IsDate = new Regex(
            @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$",
            RegexOptions.Compiled);

        //是否是Decimal
        private static readonly Regex regex_IsDecimalFraction =
            new Regex(@"^([0-9]{1,10})\.([0-9]{1,10})$", RegexOptions.Compiled);

        //是否是Double
        private static readonly Regex regex_IsDouble =
            new Regex(@"^([0-9])[0-9]*(\.\w*)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //是否是长日期
        private static readonly Regex regex_IsLongDate = new Regex(
            @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$",
            RegexOptions.Compiled);

        //是否是负数
        private static readonly Regex regex_IsNegativeInt = new Regex(@"^-\d+$", RegexOptions.Compiled);

        //只能输入字母、数字、下划线和汉字
        private static readonly Regex regex_IsE_N_UL_CN =
            new Regex(@"^[a-zA-Z\u4e00-\u9fa5\d_]+$", RegexOptions.Compiled);

        //是否是数字
        private static readonly Regex regex_IsNumeric = new Regex("^[-]?[0-9]*[.]?[0-9]*$", RegexOptions.Compiled);

        //是否是正数
        private static readonly Regex regex_IsPositiveInt = new Regex(@"^\d+$", RegexOptions.Compiled);

        //是否是短日期
        private static readonly Regex regex_IsShortDate = new Regex(
            @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$",
            RegexOptions.Compiled);

        //只能输入字母、数字、下划线
        private static readonly Regex regex_IsE_N_UL = new Regex(@"^[a-zA-Z\d_]+$", RegexOptions.Compiled);

        //是否是SQL语句
        private static readonly Regex regex_SqlFormat =
            new Regex(
                @"\?|select%20|select\s+|insert%20|insert\s+|delete%20|delete\s+|count\(|drop%20|drop\s+|update%20|update\s+",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region 基础数据验证

        /// <summary>
        ///     检测数据结果集(DataSet)是否包含数据
        /// </summary>
        /// <param name="ds">DataSet 对象</param>
        /// <returns>如果包含数据，返回true；否则返回false</returns>
        public static bool CheckedDataSet(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
                foreach (DataTable dt in ds.Tables)
                    if (dt.Rows.Count > 0)
                        return true;

            return false;
        }

        /// <summary>
        ///     检测数据表(DataTable)是否包含数据
        /// </summary>
        /// <param name="dt">DataTable 对象</param>
        /// <returns>如果包含数据，返回true；否则返回false</returns>
        public static bool CheckedDataTable(DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0;
        }

        /// <summary>
        ///     检测数组是否存在值
        /// </summary>
        /// <param name="obj">数组</param>
        /// <returns>如果包含数据，返回true；否则返回false</returns>
        public static bool CheckedObjcetArray(object[] obj)
        {
            return obj != null && obj.Length > 0;
        }

        /// <summary>
        ///     检测可枚举对象是否包含值
        /// </summary>
        /// <param name="enumerable">可枚举对象</param>
        /// <returns>如果可枚举对象包含值返回 true；否则返回 false</returns>
        public static bool CheckEnumerableObject(IEnumerable enumerable)
        {
            if (enumerable == null) return false;
            var enumerator = enumerable.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext()) count++;
            return count > 0;
        }

        /// <summary>
        ///     验证对象非null或空
        /// </summary>
        /// <param name="expVal">对象</param>
        /// <returns>如果为null或空返回true；否则返回false</returns>
        public static bool IsNotNull(object expVal)
        {
            return !IsNull(expVal);
        }

        /// <summary>
        ///     验证对象是否为null或空
        /// </summary>
        /// <param name="expVal">对象</param>
        /// <returns>如果为null或空返回true；否则返回false</returns>
        public static bool IsNull(object expVal)
        {
            if (expVal == null) return true;
            var name = expVal.GetType().Name;
            if (name == "String[]")
            {
                var strArray = (string[]) expVal;
                return strArray.Length == 0;
            }

            var str2 = expVal.ToString();
            return str2 == "";
        }

        /// <summary>
        ///     是不是Base64编码字符串
        /// </summary>
        /// <param name="expression">字符串表达式</param>
        /// <returns>如果是Base64编码字符串，返回 true; 否则，返回false</returns>
        public static bool IsBase64String(string expression)
        {
            return Regex.IsMatch(expression, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        ///     是否是中文字符串
        /// </summary>
        /// <param name="expression">字符串表达式</param>
        /// <returns>如果全是中文字符，返回true；否则，返回false</returns>
        public static bool IsCnChar(string expression)
        {
            return Regex.IsMatch(expression, "^(?:[一-龥])+$");
        }

        /// <summary>
        ///     是否是Unicode字符串
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>如果是Unicode字符串返回true；否则返回false</returns>
        public static bool IsUnicode(string s)
        {
            var pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        ///     检测字符串是否是日期
        /// </summary>
        /// <param name="dateval">日期字符串</param>
        /// <returns>是日期格式返回 true; 否则返回 false</returns>
        public static bool IsDate(string dateval)
        {
            return regex_IsDate.IsMatch(dateval);
        }

        /// <summary>
        ///     检测字符串是不是有效的长日期
        /// </summary>
        /// <param name="dateval">日期字符串</param>
        /// <returns>是长日期格式返回 true; 否则返回 false</returns>
        public static bool IsLongDate(string dateval)
        {
            return regex_IsLongDate.IsMatch(dateval);
        }

        /// <summary>
        ///     检测字符串是不是有效的短日期
        /// </summary>
        /// <param name="dateval">日期字符串</param>
        /// <returns>是短日期格式返回 true; 否则返回 false</returns>
        public static bool IsShortDate(string dateval)
        {
            return regex_IsShortDate.IsMatch(dateval);
        }

        /// <summary>
        ///     检测字符串是不是有效的时间
        /// </summary>
        /// <param name="timeval">时间字符串</param>
        /// <returns>是时间格式返回 true; 否则返回 false</returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, "^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        /// <summary>
        ///     检测字符串是否是 Decimal
        /// </summary>
        /// <param name="expression">Decimal 字符串</param>
        /// <returns>如果是 Decimal格式的返回 true; 否则，返回 false</returns>
        public static bool IsDecimalFraction(string expression)
        {
            return regex_IsDecimalFraction.IsMatch(expression);
        }

        /// <summary>
        ///     检测字符串是否是双精度浮点数
        /// </summary>
        /// <param name="expression">Double 字符串</param>
        /// <returns>如果是 Double格式的返回 true; 否则，返回 false</returns>
        public static bool IsDouble(object expression)
        {
            return expression != null && regex_IsDouble.IsMatch(expression.ToString());
        }

        /// <summary>
        ///     检测字符串是否是有效的数值
        /// </summary>
        /// <param name="expression">字符串</param>
        /// <returns>验证成功返回 true; 验证失败返回 false</returns>
        public static bool IsNumeric(object expression)
        {
            var input = expression?.ToString();
            return input?.Length > 0 && input.Length <= 11 && regex_IsNumeric.IsMatch(input) &&
                   (input.Length < 10 || input.Length == 10 && input[0] == '1' ||
                    input.Length == 11 && input[0] == '-' && input[1] == '1');
        }

        /// <summary>
        ///     判断给定的字符串数组(expression)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返回 true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null) return false;
            return strNumber.Length >= 1 && strNumber.All(IsNumeric);
        }

        /// <summary>
        ///     检测字符串是否是有效的32位整型(负数)
        /// </summary>
        /// <param name="expression">字符串</param>
        /// <returns>验证成功返回 true; 验证失败返回 false</returns>
        public static bool IsNegativeInt(string expression)
        {
            return regex_IsNegativeInt.Match(expression).Success && long.Parse(expression) >= -2147483648L;
        }

        /// <summary>
        ///     检测字符串是否是有效的32位整型(正数)
        /// </summary>
        /// <param name="expression">字符串</param>
        /// <returns>验证成功返回 true; 验证失败返回 false</returns>
        public static bool IsPositiveInt(string expression)
        {
            return regex_IsPositiveInt.Match(expression).Success && long.Parse(expression) <= 0x7fffffffL;
        }

        /// <summary>
        ///     检测字符串是否是有效的64位长整型(正数)
        /// </summary>
        /// <param name="expression">字符串</param>
        /// <returns>验证成功返回 true; 验证失败返回 false</returns>
        public static bool IsPositiveInt64(string expression)
        {
            return regex_IsPositiveInt.Match(expression).Success && long.Parse(expression) <= 0x7fffffffffffffffL;
        }

        /// <summary>
        ///     验证字符串是否由字母和数字组成
        /// </summary>
        /// <param name="expression">字符串</param>
        /// <returns>验证成功返回true；否则返回false</returns>
        public static bool IsE_N(string expression)
        {
            return Regex.IsMatch(expression, "[0-9a-zA-Z]?");
        }

        /// <summary>
        ///     验证字符串指定长度的子字符串是否是数字和字母
        /// </summary>
        /// <param name="expression">字符串</param>
        /// <param name="start">起始索引</param>
        /// <param name="end">结束索引</param>
        /// <returns>验证成功返回true；否则返回false</returns>
        public static bool IsSp_E_N(string expression, int start, int end)
        {
            if (string.IsNullOrEmpty(expression) || start > end) return false;
            return Regex.IsMatch(expression, "^[0-9a-zA-Z]{{" + start + "},{" + end + "}}$");
        }

        /// <summary>
        ///     是否是中文字符、字母、数字
        /// </summary>
        /// <param name="expression">字符串表达式</param>
        /// <returns>如果字符串中包含字母、数字、中文字符返回 true; 否则返回 false</returns>
        public static bool IsE_N_CN(string expression)
        {
            return Regex.IsMatch(expression, "^[0-9a-zA-Z一-龥]+$");
        }

        /// <summary>
        ///     验证输入是否为字母、数字和下划线的组合
        /// </summary>
        /// <param name="strVal">字符串</param>
        /// <returns>验证成功返回 true; 失败返回 false</returns>
        public static bool IsE_N_UL(string strVal)
        {
            return regex_IsE_N_UL.IsMatch(strVal);
        }

        /// <summary>
        ///     验证输入是否为字母、数字、下划线和汉字的组合
        /// </summary>
        /// <param name="strVal">字符串</param>
        /// <returns>验证成功返回 true; 失败返回 false</returns>
        public static bool IsE_N_UL_CN(string strVal)
        {
            return regex_IsE_N_UL_CN.IsMatch(strVal);
        }

        /// <summary>
        ///     通用正则表达式判断函数
        /// </summary>
        /// <param name="strVerifyString">用于匹配的字符串</param>
        /// <param name="strRegular">正则表达式</param>
        /// <param name="regOption">配置正则表达式的选项</param>
        /// <param name="aryResult">分解的字符串内容</param>
        /// <param name="IsEntirety">是否需要完全匹配</param>
        /// <returns>匹配成功返回 true; 匹配失败返回false</returns>
        public static bool CommRegularMatch(string strVerifyString, string strRegular, RegexOptions regOption,
            ref ArrayList aryResult, bool IsEntirety)
        {
            //如果需要完全匹配的处理
            if (IsEntirety)
            {
                strRegular = strRegular.Insert(0, @"\A");
                strRegular = strRegular.Insert(strRegular.Length, @"\z");
            }

            var r = new Regex(strRegular, regOption);
            for (var m = r.Match(strVerifyString); m.Success; m = m.NextMatch()) aryResult.Add(m);

            return aryResult.Count != 0;
        }

        #endregion

        #region 网络格式验证

        /// <summary>
        ///     检测字符串是否是域名
        /// </summary>
        /// <param name="strHost">字符串</param>
        /// <returns>匹配成功返回 true; 否则返回 false</returns>
        public static bool IsDomain(string strHost)
        {
            var regex = new Regex(@"^\d+$");
            if (strHost.IndexOf(".", StringComparison.Ordinal) == -1) return false;
            return !regex.IsMatch(strHost.Replace(".", string.Empty));
        }

        /// <summary>
        ///     检测IP地址是否合法
        /// </summary>
        /// <param name="ipval">IP地址</param>
        /// <returns>验证成功返回 true; 否则返回 false</returns>
        public static bool IsIP(string ipval)
        {
            return Regex.IsMatch(ipval, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        ///     检测IP地址和端口号是否合法
        /// </summary>
        /// <param name="ipval">IP地址</param>
        /// <returns></returns>
        public static bool IsIPAndPort(string ipval)
        {
            return Regex.IsMatch(ipval,
                @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9]),\d{1,5}?$");
        }

        /// <summary>
        ///     检测是否是IP协议
        /// </summary>
        /// <param name="ipval">IP地址</param>
        /// <returns>匹配成功返回 true; 否则返回 false</returns>
        public static bool IsIPSect(string ipval)
        {
            return Regex.IsMatch(ipval,
                @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }

        /// <summary>
        ///     检测URL是否是有效的地址
        /// </summary>
        /// <param name="strUrl">URL地址</param>
        /// <returns>匹配成功返回 true；否则返回 false</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl,
                @"^(http|https|ftp)\://(((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])|([a-zA-Z0-9_\-\.])+\.(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum|uk|me))((:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*)$");
        }

        /// <summary>
        ///     检测字符串是否为物理路径
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>如果是物理路径返回true; 否则返回false</returns>
        public static bool IsPhysicalPath(string s)
        {
            var pattern = @"^\s*[a-zA-Z]:.*$";
            return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        ///     检测字符串是否为相对路径
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>如果是物理路径返回true; 否则返回false</returns>
        public static bool IsRelativePath(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            if (s.StartsWith("/") || s.StartsWith("?")) return false;
            return !Regex.IsMatch(s, @"^\s*[a-zA-Z]{1,10}:.*$");
        }

        #endregion

        #region 文件格式验证

        /// <summary>
        ///     检测文件名是否合法
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>匹配成功返回 true, 否则返回 false</returns>
        public static bool IsFileName(string filename)
        {
            return !regex_FileNameFilter.IsMatch(filename);
        }

        /// <summary>
        ///     检测是否是有效的图片格式
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>验证成功返回 true；否则返回 false</returns>
        public static bool IsImage(string filename)
        {
            return regex_ImgFormat.Match(filename).Success;
        }

        #endregion

        #region 生活服务验证

        /// <summary>
        ///     检测字符串是否为合法的电子邮箱
        ///     由于邮件服务商众多，这里只是一个通用的邮件验证
        /// </summary>
        /// <param name="strEmail">电子邮箱</param>
        /// <returns>匹配成功返回 true, 否则返回 false</returns>
        public static bool IsEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        /// <summary>
        ///     检测身份证是否合法
        ///     限中国二代身份证，2013年1月1日起，一代身份证停止使用
        /// </summary>
        /// <param name="cardno">身份证号码</param>
        /// <returns>验证成功返回true; 否则返回false</returns>
        public static bool IsIdCardNo(string cardno)
        {
            var length = cardno.Length;
            if (length != 18) return false;
            var factors = new[] {7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1};
            var array = new string[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = cardno[i].ToString();
                if ((cardno[i] < '0' || cardno[i] > '9') && i != 17) return false;
                if (i < 17) array[i] = (uint.Parse(cardno[i].ToString()) * factors[i]).ToString();
            }

            var birthday = cardno.Substring(6, 8);
            birthday = string.Format("{0}-{1}-{2}",
                birthday.Substring(0, 4),
                birthday.Substring(4, 2),
                birthday.Substring(6, 2)
            );
            if (!IsDate(birthday)) return false;
            var lngProduct = 0;
            for (var i = 0; i < 17; i++) lngProduct += int.Parse(array[i]);
            var checkDigit = 12 - lngProduct % 11;
            var last = checkDigit.ToString();
            switch (checkDigit)
            {
                case 10:
                    last = "X";
                    break;
                case 11:
                    last = "0";
                    break;
                case 12:
                    last = "1";
                    break;
            }

            if (last != cardno[17].ToString()) return false;
            return true;
        }

        /// <summary>
        ///     检测身份证是否合法
        ///     一代身份证自动转换为二代身份证
        /// </summary>
        /// <param name="cardno">身份证号码</param>
        /// <param name="message">身份证信息</param>
        /// <returns>验证成功返回true; 否则返回false</returns>
        public static bool IsIdCardNo(string cardno, out string message)
        {
            if (!(cardno.Length != 15 || cardno.Length != 18))
            {
                message = "身份证长度不合法。";
                return false;
            }

            if (cardno.Length == 15) cardno = IdCardUpdate(cardno);
            return CheckIdCard(cardno, out message);
        }

        /// <summary>
        ///     检测身份证是否合法
        /// </summary>
        /// <param name="cid">身份证号码</param>
        /// <param name="message">输出消息</param>
        /// <returns>验证成功返回true; 否则返回false</returns>
        private static bool CheckIdCard(string cid, out string message)
        {
            var aCity = new[]
            {
                null, null, null, null, null, null, null, null, null, null, null, "北京", "天津", "河北", "山西", "内蒙古", null,
                null, null, null, null, "辽宁", "吉林", "黑龙江", null, null, null, null, null, null, null, "上海", "江苏", "浙江",
                "安微", "福建", "江西", "山东", null, null, null, "河南", "湖北", "湖南", "广东", "广西", "海南", null, null, null, "重庆",
                "四川", "贵州", "云南", "西藏", null, null, null, null, null, null, "陕西", "甘肃", "青海", "宁夏", "新疆", null, null,
                null, null, null, "台湾", null, null, null, null, null, null, null, null, null, "香港", "澳门", null, null,
                null, null, null, null, null, null, "国外"
            };
            double iSum = 0;
            message = string.Empty;
            var rg = new Regex(@"^\d{17}(\d|x)$");
            var mc = rg.Match(cid);
            if (!mc.Success) return false;
            cid = cid.ToLower();
            cid = cid.Replace("x", "a");
            if (aCity[int.Parse(cid.Substring(0, 2))] == null)
            {
                message = "非法地区";
                return false;
            }

            try
            {
                DateTime.Parse(cid.Substring(6, 4) + " - " + cid.Substring(10, 2) + " - " + cid.Substring(12, 2));
            }
            catch
            {
                message = "非法生日";
                return false;
            }

            for (var i = 17; i >= 0; i--)
                iSum += Math.Pow(2, i) % 11 * int.Parse(cid[17 - i].ToString(), NumberStyles.HexNumber);
            if (iSum % 11 != 1)
            {
                message = "非法证号";
                return false;
            }

            message = aCity[int.Parse(cid.Substring(0, 2))] + "," + cid.Substring(6, 4) + "-" +
                      cid.Substring(10, 2) + "-" + cid.Substring(12, 2) + "," +
                      (int.Parse(cid.Substring(16, 1)) % 2 == 1 ? "男" : "女");
            return true;
        }

        /// <summary>
        ///     身份证号码由15位升级到18位
        /// </summary>
        /// <param name="IdCard15">15位身份证号码</param>
        /// <returns>18位身份证号码</returns>
        public static string IdCardUpdate(string IdCard15)
        {
            //第十八位校验码
            char[] verify = {'1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2'};
            //因子
            int[] factors = {7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1};
            var intTemp = 0;

            var strTemp = IdCard15.Substring(0, 6) + "19" + IdCard15.Substring(6);
            for (var i = 0; i <= strTemp.Length - 1; i++) intTemp += int.Parse(strTemp.Substring(i, 1)) * factors[i];
            intTemp = intTemp % 11;
            return strTemp + verify[intTemp];
        }

        /// <summary>
        ///     检测手机号码是否合法
        /// </summary>
        /// <param name="strMobile">手机号码</param>
        /// <returns>验证成功返回 true; 失败返回 false</returns>
        public static bool IsMobileCode(string strMobile)
        {
            return Regex.IsMatch(strMobile, @"^((\(\d{2,3}\))|(\d{3}\-))?((13\d{9})|(15[389]\d{8})|(18\d{9}))$");
        }

        /// <summary>
        ///     检测电话号码是否合法
        /// </summary>
        /// <param name="strPhone">电话号码</param>
        /// <returns>验证成功返回 true; 失败返回 false</returns>
        public static bool IsPhoneCode(string strPhone)
        {
            return Regex.IsMatch(strPhone, @"^(86)?(-)?(0\d{2,3})?(-)?(\d{7,8})(-)?(\d{3,5})?$");
        }

        /// <summary>
        ///     验证邮编是否合法
        /// </summary>
        /// <param name="strPostalCode">邮编</param>
        /// <returns>验证成功返回 true; 失败返回 false</returns>
        public static bool IsPostalCode(string strPostalCode)
        {
            return Regex.IsMatch(strPostalCode, @"^\d{6}$");
        }

        /// <summary>
        ///     验证腾讯QQ是否合法
        /// </summary>
        /// <param name="qq">QQ号码</param>
        /// <returns>验证成功返回 true; 失败返回 false</returns>
        public static bool IsTencentQQ(string qq)
        {
            return Regex.IsMatch(qq, @"[1-9][0-9]{4,}");
        }

        #endregion

        #region 字符串安全验证

        /// <summary>
        ///     检测输入是否安全
        /// </summary>
        /// <param name="expression">字符串</param>
        /// <returns>输入字符串安全返回 true；否则返回 false</returns>
        public static bool IsSafeInputWords(string expression)
        {
            return Regex.IsMatch(expression, "/^\\s*$|^c:\\\\con\\\\con$|[%,\\*\"\\s\\t\\<\\>\\&]|$guestexp/is");
        }

        /// <summary>
        ///     验证内容是否安全。防止SQL注入
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>如果包含SQL注入返回false; 没有包含返回true</returns>
        public static bool IsSafety(string s)
        {
            var input = Regex.Replace(s.Replace("%20", " "), @"\s", " ");
            const string pattern =
                "select |insert |delete from |count\\(|drop table|update |truncate |asc\\(|mid\\(|char\\(|xp_cmdshell|exec master|net localgroup administrators|:|net user|\"|\\'| or ";
            return !Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     验证字符串是否是SQL语句
        /// </summary>
        /// <param name="sqlExpression">SQL语句</param>
        /// <returns>验证成功返回true；否则返回false</returns>
        public static bool IsSQL(string sqlExpression)
        {
            return regex_SqlFormat.IsMatch(sqlExpression);
        }

        /// <summary>
        ///     验证字符串是否合法
        /// </summary>
        /// <param name="expression">字符串</param>
        /// <returns>合法返回true；否则返回false</returns>
        private static bool IsLicitStr(string expression)
        {
            const string filterStr =
                ";|'|&|%|--|==|<|>|*|(|)| create | select | count | insert | update | drop | from | declare | exec | char |xp_cmdshell| where | or | and | begin |truncate| union | join |script";
            expression = expression.ToLower();
            var filterArray = filterStr.Split('|');
            return filterArray.All(filter => expression.IndexOf(filter, StringComparison.Ordinal) == -1);
        }

        #endregion
    }
}