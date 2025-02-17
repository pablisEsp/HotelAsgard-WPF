using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HotelAsgard.Models.Rooms;

namespace HotelAsgard.Views.RoomsViews;

public partial class NewCategory : INotifyPropertyChanged
{
    public Category NuevaCategoria { get; private set; }
    private const int MAX_HABITACION_SIZE = 10000;
    private const int MAX_HUESPEDES = 50;
    private const int MAX_CAMAS = 10;
    private const int MAX_PRICE = 100000;

    private bool _isFormValid;

    public bool IsFormValid
    {
        get => _isFormValid;
        set
        {
            if (_isFormValid != value)
            {
                _isFormValid = value;
                OnPropertyChanged(nameof(IsFormValid));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public NewCategory()
    {
        InitializeComponent();
        lstServicios.ItemsSource = ServiceList.ServiciosDisponibles; // Carga los servicios en la UI
        DataContext = this; // Enlaza IsFormValid con el XAML

        // Reiniciar la selección de los servicios cada vez que se abre la ventana
        foreach (var servicio in lstServicios.Items.Cast<Service>())
        {
            servicio.Seleccionado = false;
        }
        
        // Una vez que la ventana está cargada, se puede validar sin riesgo de null.
        Loaded += (s, e) => ValidateForm();
    }

    private void Guardar_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombreCategoria.Text))
        {
            MessageBox.Show("Debe ingresar un nombre para la categoría.", "Error", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(txtTamanyo.Text, out int tamanyo) || tamanyo <= 0 ||
            !int.TryParse(txtNumHuespedes.Text, out int numPersonas) || numPersonas <= 0 ||
            !int.TryParse(txtPrecio.Text, out int precio) || precio < 0)
        {
            MessageBox.Show("Ingrese valores numéricos válidos.", "Error", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        // Obtener camas ingresadas
        List<Bed> camas = ObtenerCamas();

        // Obtener servicios seleccionados
        List<string> servicios = lstServicios.Items.Cast<Service>()
            .Where(s => s.Seleccionado)
            .Select(s => s.Nombre)
            .ToList();

        // Crear objeto de categoría
        NuevaCategoria = new Category
        {
            Nombre = txtNombreCategoria.Text,
            Tamanyo = tamanyo,
            NumPersonas = numPersonas,
            Precio = precio,
            Camas = camas,
            Servicios = servicios
        };

        DialogResult = true;
        this.Close();
    }

    private List<Bed> ObtenerCamas()
    {
        List<Bed> camas = new List<Bed>();

        AgregarCama("King", txtCamasKing, camas);
        AgregarCama("Queen", txtCamasQueen, camas);
        AgregarCama("Double", txtCamasDouble, camas);
        AgregarCama("Twin", txtCamasTwin, camas);
        AgregarCama("Sofá Cama", txtCamasSofa, camas);

        return camas;
    }

    private void AgregarCama(string nombre, TextBox txtBox, List<Bed> listaCamas)
    {
        if (int.TryParse(txtBox.Text, out int num) && num > 0)
        {
            listaCamas.Add(new Bed
            {
                Nombre = nombre,
                NumCamas = num
            }); // No se asigna _id
        }
    }

    private void Cancelar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        this.Close();
    }

    // Validación para permitir solo números
    private void OnlyNumbers(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$"); // Solo números
    }

    // Validación para asegurar límites en tamaño, huéspedes y camas
    private void MaxValueCheck(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            if (int.TryParse(textBox.Text, out int value))
            {
                int newValue = value; // Variable para almacenar el valor corregido

                if (textBox.Name == "txtPrecio" && value > MAX_PRICE)
                {
                    newValue = MAX_PRICE; // Límite 100,000 €
                }
                else if (textBox.Name == "txtTamanyo" && value > MAX_HABITACION_SIZE)
                {
                    newValue = MAX_HABITACION_SIZE; // Límite 10,000m²
                }
                else if (textBox.Name == "txtNumHuespedes" && value > MAX_HUESPEDES)
                {
                    newValue = MAX_HUESPEDES; // Límite 50 huéspedes
                }
                else if (textBox.Name.Contains("Camas") && value > MAX_CAMAS)
                {
                    newValue = MAX_CAMAS; // Límite 10 camas por tipo
                }

                // Solo actualizamos el TextBox si realmente cambió el valor
                if (value != newValue)
                {
                    textBox.Text = newValue.ToString();
                    textBox.CaretIndex = textBox.Text.Length; // Mueve el cursor al final
                }
            }
            else
            {
                textBox.Text = "0"; // Si el valor no es numérico, lo pone en 0
                textBox.CaretIndex = textBox.Text.Length;
            }

            // Llamamos a la validación general
            ValidateForm();
        }
    }


    // Valida si todos los campos obligatorios están completos
    private void ValidateForm()
    {
        // Si alguno de los controles aún no está inicializado, salimos.
        if (txtNombreCategoria == null ||
            txtTamanyo == null ||
            txtNumHuespedes == null ||
            txtPrecio == null ||
            txtCamasKing == null ||
            txtCamasQueen == null ||
            txtCamasDouble == null ||
            txtCamasTwin == null ||
            txtCamasSofa == null)
        {
            return;
        }

        // Validación del nombre (obligatorio)
        bool nombreValido = !string.IsNullOrWhiteSpace(txtNombreCategoria.Text);

        // Validación de tamaño y número de huéspedes (obligatorios y > 0)
        bool tamanyoValido = !string.IsNullOrWhiteSpace(txtTamanyo.Text) &&
                             int.TryParse(txtTamanyo.Text, out int tamanyo) && tamanyo > 0;
        bool huespedesValido = !string.IsNullOrWhiteSpace(txtNumHuespedes.Text) &&
                               int.TryParse(txtNumHuespedes.Text, out int huespedes) && huespedes > 0;

        // Validación del precio (puede ser 0 o mayor)
        bool precioValido = !string.IsNullOrWhiteSpace(txtPrecio.Text) &&
                            int.TryParse(txtPrecio.Text, out int precio) && precio >= 0;

        // Validación de camas: al menos una debe ser > 0
        bool camasValido =
            (int.TryParse(txtCamasKing.Text, out int camasKing) && camasKing > 0) ||
            (int.TryParse(txtCamasQueen.Text, out int camasQueen) && camasQueen > 0) ||
            (int.TryParse(txtCamasDouble.Text, out int camasDouble) && camasDouble > 0) ||
            (int.TryParse(txtCamasTwin.Text, out int camasTwin) && camasTwin > 0) ||
            (int.TryParse(txtCamasSofa.Text, out int camasSofa) && camasSofa > 0);

        // Actualiza la propiedad IsFormValid basándose en todas las validaciones
        IsFormValid = nombreValido && tamanyoValido && huespedesValido && precioValido && camasValido;
    }

    
    private void InputChanged(object sender, TextChangedEventArgs e)
    {
        ValidateForm();
    }


}