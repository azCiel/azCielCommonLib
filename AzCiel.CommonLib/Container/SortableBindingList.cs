/*
 * Copyright (c) 2008 az'Ciel HAKKO Co.,Ltd.
 * All Rights Reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

/*
 * ソート可能な BindinList<> クラス定義
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AzCiel.CommonLib.Container {

    /// <summary>
    /// ソート可能な BindingList
    /// </summary>
    /// <remarks>
    /// DataGridView に BindingSorce 経由でバインドした際に、自動 Sort を可能にする
    /// ための BindinList
    /// </remarks>
    /// <typeparam name="T">要素型</typeparam>
    public class SortableBindingList<T> : BindingList<T> {

        private PropertyDescriptor sortProperty_ = null;
        private ListSortDirection sortDirection_ = ListSortDirection.Ascending;
        private bool isSorted_ = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SortableBindingList() {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="list">データソース</param>
        public SortableBindingList(IList<T> list) : base(list) {
        }

        /// <summary>
        /// 要素列の再設定
        /// </summary>
        /// <param name="list">要素列</param>
        public void ReplaceAllItems(IList<T> list) {
            Clear();
            foreach (T item in list) {
                Add(item);
            }
        }

        /// <summary>
        /// ソート処理本体
        /// </summary>
        /// <param name="property">プロパティ</param>
        /// <param name="direction">ソート方向</param>
        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction) {
            List<T> list = Items as List<T>;
            if (list != null) {
                list.Sort(PropertyComparerFactory.Factory<T>(property, direction));

                this.isSorted_ = true;
                this.sortProperty_ = property;
                this.sortDirection_ = direction;

                this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        /// <summary>
        /// ソートサポートフラグ
        /// </summary>
        protected override bool SupportsSortingCore {
            get {
                return true;
            }
        }

        /// <summary>
        /// ソート解除 (非対応)
        /// </summary>
        protected override void RemoveSortCore() {
            throw new NotSupportedException();
        }

        /// <summary>
        /// ソート済みか？
        /// </summary>
        protected override bool IsSortedCore {
            get {
                return this.isSorted_;
            }
        }

        /// <summary>
        /// ソートプロパティ
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore {
            get {
                return this.sortProperty_;
            }
        }

        /// <summary>
        /// ソート方向
        /// </summary>
        protected override ListSortDirection SortDirectionCore {
            get {
                return this.sortDirection_;
            }
        }

    }

    /// <summary>
    /// オブジェクトプロパティ Comparer のファクトリ
    /// </summary>
    public static class PropertyComparerFactory {

        /// <summary>
        /// ファクトリメソッド
        /// </summary>
        /// <typeparam name="T">要素クラス</typeparam>
        /// <param name="property">プロパティ</param>
        /// <param name="direction">ソート方向</param>
        /// <returns>オブジェクトプロパティ Comparer オブジェクト</returns>
        public static IComparer<T> Factory<T>(PropertyDescriptor property, ListSortDirection direction) {
            Type seed = typeof(PropertyComparer<,>);
            Type[] typeArgs = { typeof(T), property.PropertyType };
            Type pcType = seed.MakeGenericType(typeArgs);

            IComparer<T> comparer = (IComparer<T>)Activator.CreateInstance(pcType, new object[] { property, direction });
            return comparer;
        }

    }

    /// <summary>
    /// オブジェクトプロパティ Comparer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public sealed class PropertyComparer<T, U> : IComparer<T> {

        private PropertyDescriptor property_;
        private ListSortDirection direction_;
        private Comparer<U> comparer_;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="property">プロパティ</param>
        /// <param name="direction">比較方向</param>
        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction) {
            this.property_ = property;
            this.direction_ = direction;
            this.comparer_ = Comparer<U>.Default;
        }

        /// <summary>
        /// 比較メソッド
        /// </summary>
        /// <param name="x">左辺オブジェクト</param>
        /// <param name="y">右辺オブジェクト</param>
        /// <returns>比較結果</returns>
        public int Compare(T x, T y) {
            U xValue = (U)this.property_.GetValue(x);
            U yValue = (U)this.property_.GetValue(y);

            if (this.direction_ == ListSortDirection.Ascending)
                return this.comparer_.Compare(xValue, yValue);
            else
                return this.comparer_.Compare(yValue, xValue);
        }

    }
}
/*
 * -*- settings for emacs. -*-
 * Local Variables:
 *   tab-width: 4
 *   indent-tabs-mode: nil
 *   c-basic-offset: 4
 * End:
 */
