## ComputerGraphics Filters

Лабораторная работа по курсу компьютерной графики.

Работу выполнили: [Damlrca](https://github.com/Damlrca), [Mortus19](https://github.com/Mortus19), [aartyomm](https://github.com/aartyomm)

### Примеры фильтров

***Инверсия***

|![borsch](sample_images/borsch.png)|![inverted borsch](sample_images/borsch_invert.png)|
|-|-|

***Размытие по Гауссу***

|![butterfly](sample_images/butterfly.png)|![gaussian blur butterfly](sample_images/butterfly_gauss.png)|
|-|-|

***Выделение границ (Оператор Собеля)***

|![conrod](sample_images/conrod.png)|![sobel operator conrod](sample_images/conrod_sobel.png)|
|-|-|

***Тиснение***

|![columns](sample_images/columns.png)|![embossing columns](sample_images/columns_embossing.png)|
|-|-|

***Медианный фильтр (подавление шума "соль и перец")***

|![noisy crab](sample_images/crab_noisy.png)|![median filter crab](sample_images/crab_median.png)|
|-|-|

***Линейное растяжение гистограммы***

|![snowtree](sample_images/snowtree.png)|![autolevels snowtree](sample_images/snowtree_autolevels.png)|
|-|-|

***Оттенки серого + Дизеринг***

|![scott](sample_images/scott.png)|![dithering scott](sample_images/scott_dither.png)|
|-|-|

***Статистическая цветокоррекция***

|Исходное|Целевое|Результат|
|:-|:-|:-|
|![fallout](sample_images/fallout.png)|![fallout color](sample_images/fallout_color.png)|![fallout result](sample_images/fallout_result.png)|

***Компенсация разности освещения***

|![diffeq](sample_images/diffeq.png)|![diffeq_comp](sample_images/diffeq_comp.png)|
|-|-|

### Список реализованных фильтров

- Точечные фильтры
	- Инверсия
	- Оттенки серого
	- Сепия
	- Увеличение яркости
- Матричные фильтры
	- Размытие
		- Прямоугольное размытие
		- Размытие в движении
		- Размытие по Гауссу
	- Выделение границ
		- Оператор Прюитт
		- Оператор Собеля
		- Оператор Щарра
	- Повышение резкости
	- Тиснение
	- Компенсация разности освещения
- Геометрические фильтры
	- Поворот
	- Перенос
	- "Волны"
	- "Стекло"
- Нелинейные фильтры
	- Медианный
	- Максимальный
	- Минимальный
- Глобальные фильтры
	- Повышение контрастности
	- Линейное растяжение гистограммы
	- "Серый мир"
	- "Идеальный отражатель"
	- Коррекция с опорным цветом
	- Статистическая цветокоррекция
- Добавление шумов
	- "Соль и перец"
- Квантование и дизеринг
- Морфологические фильтры
	- Расширение
	- Сужение
	- Открытие
	- Закрытие
	- Top Hat
	- Black Hat
	- Grad

А также разбиение на каналы RGB, YIQ, CMY, CMYK.

### Другие особенности работы

- Возможность сохранить обработанное изображение
- Возможность отменить последний примененный фильтр
- Возможность повторить последний примененный фильтр
- Пропорциональное изменение размеров элементов при изменении размера окна
