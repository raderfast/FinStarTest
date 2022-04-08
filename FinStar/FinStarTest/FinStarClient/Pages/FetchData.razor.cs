using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Models;
using Newtonsoft.Json;
using RestSharp;
using Parameter = Models.Parameter;

namespace FinStarClient.Pages
{
    public partial class FetchData
    {
        /// <summary>
        /// Клиент для взаимодействия с сервисом
        /// </summary>
        [Inject]
        private RestClient _restClient { get; set; }

        /// <summary>
        /// Набор данных
        /// </summary>
        private List<ValueSet> ValueSets = new List<ValueSet>();

        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        private int _page = 1;
        /// <summary>
        /// Количество отображаемых элементов
        /// </summary>
        private int _itemsOnPage = 5;
        /// <summary>
        /// Количество страниц
        /// </summary>
        private int _pageCount = 3;
        /// <summary>
        /// Всего элементов
        /// </summary>
        private int _totalItems = 0;

        private int _tempColOne = 0;
        private int _tempColTwo = 0;
        private string _tempColThree = "";

        private string? _filerString = String.Empty;

        /// <summary>
        /// Флаг редактирования строки
        /// </summary>
        public bool RowBeingEdited { get; set; } = false;

        /// <summary>
        /// Флаг редактирования списка
        /// </summary>
        public bool ListEdit { get; set; } = false;

        /// <summary>
        /// Флаг нового элемента
        /// </summary>
        public bool NewRecord { get; set; } = false;



        /// <summary>
        /// Список элементов с флагами их редактирования
        /// </summary>
        public Dictionary<int, (ValueSet data, bool editMode)> GridData { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadData();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <returns></returns>
        private async Task LoadData()
        {
            try
            {
                var parameter = await _restClient.GetAsync<List<Parameter>>(
                                    new RestRequest(@"https://localhost:7136/TestService/GetValuesCount")) ??
                                new List<Parameter>();

                if (parameter.Find(p => p.Name == "IsError")?.Value == "false")
                {
                    if (Int32.TryParse(parameter.Find(p => p.Name == "ValuesCount")?.Value, out _totalItems))
                    {
                        _pageCount = Convert.ToInt32(Math.Ceiling((double)_totalItems / _itemsOnPage));
                    }
                }

                ValueSets = await _restClient.GetAsync<List<ValueSet>>(
                                new RestRequest(@"https://localhost:7136/TestService/GetValues")
                                    .AddHeader("filterData", _filerString ?? String.Empty)
                                    .AddHeader("skip", (_page-1) * _itemsOnPage)
                                    .AddHeader("take", _itemsOnPage)
                            )
                            ?? new List<ValueSet>();

                GridData.Clear();

                foreach (var valueSet in ValueSets)
                {
                    GridData.Add(valueSet.OrderNumber, (data: valueSet, editMode: false));
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Отправка данных
        /// </summary>
        /// <returns></returns>
        private async Task PostData()
        {
            //var response = await _restClient.PostJsonAsync(@"https://localhost:7136/TestService/PostValues", ValueSets);

            var response = await _restClient.PostAsync(new RestRequest(@"https://localhost:7136/TestService").AddBody(ValueSets));

            if (response.Content != null)
            {
                var parameters = JsonConvert.DeserializeObject<List<Parameter>>(response.Content);
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await LoadData();
            }
        }

        private async Task FilterData(string? value)
        {
            _filerString = value;
            await LoadData();
        }

        private async Task CurrentPageChange(int currentPage)
        {
            _page = currentPage;
            await LoadData();
        }

        private async Task ItemsOnPageChange(int itemCount)
        {
            _itemsOnPage = itemCount;
            await LoadData();
        }

        /// <summary>
        /// Добавление нового элемента
        /// </summary>
        protected void AddNew()
        {
            if (RowBeingEdited)
            {
                // We can't go into edit mode to add new if we are already editing
                return;
            }

            ValueSet newgridItem = new();
            GridData.Add(0, (data: newgridItem, editMode: true));
            RowBeingEdited = true;
            NewRecord = true;
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="gridItemId">Идентификатор элемента</param>
        /// <returns></returns>
        protected async Task Delete(int gridItemId)
        {
            ValueSets.Remove(ValueSets.Find(vs => vs.OrderNumber == gridItemId) ?? new ValueSet());
            GridData.Remove(gridItemId);
            await PostData();
            RowBeingEdited = false;
        }

        /// <summary>
        /// Редактирование элемента
        /// </summary>
        /// <param name="gridItemId"></param>
        private void SetRowEdit(int gridItemId)
        {
            var prjType = GridData[gridItemId];
            prjType.editMode = true;
            GridData[gridItemId] = prjType;
            RowBeingEdited = true;
        }

        /// <summary>
        /// Окончание редактирования
        /// </summary>
        /// <param name="gridItemId"></param>
        private void UnSetRowEdit(int gridItemId)
        {
            var prjType = GridData[gridItemId];
            prjType.editMode = false;
            GridData[gridItemId] = prjType;
            RowBeingEdited = false;
        }

        /// <summary>
        /// Начало редактирования
        /// </summary>
        /// <param name="gridItemId"></param>
        protected void StartEditMode(int gridItemId)
        {
            SetRowEdit(gridItemId);
            _tempColOne = GridData[gridItemId].data.OrderNumber;
            _tempColTwo = GridData[gridItemId].data.Code;
            _tempColThree = GridData[gridItemId].data.Value ?? String.Empty;
        }

        /// <summary>
        /// Отмена редактирования
        /// </summary>
        /// <param name="gridItemId"></param>
        protected void CancelEditMode(int gridItemId)
        {
            UnSetRowEdit(gridItemId);
            if (NewRecord)
            {
                NewRecord = false;
                GridData.Remove(0);
                return;
            }
            GridData[gridItemId].data.OrderNumber = _tempColOne;
            GridData[gridItemId].data.Code = _tempColTwo;
            GridData[gridItemId].data.Value = _tempColThree;
        }

        /// <summary>
        /// Сохранение результата редактирования
        /// </summary>
        /// <param name="gridItemId"></param>
        /// <returns></returns>
        protected async Task SaveRowEdit(int gridItemId)
        {
            if (NewRecord)
            {
                ValueSets.Add(GridData[gridItemId].data);
                GridData.Remove(gridItemId);
                await PostData();
                NewRecord = false;
                RowBeingEdited = false;
                return;
            }

            UnSetRowEdit(gridItemId);
        }
    }
}
