using UnityEngine;
using UnityEngine.UIElements;

public class SingleItemView
{
	private const string _singleValueName = "single-value";
	
	private const string _iconContainerName = "icon-container";
	private const string _iconImageName = "icon-image";

	private const string _nameContainerName = "name-container";
	private const string _nameLabelName = "name-label";

	private const string _amountContainerName = "amount-container";
	private const string _amountLabelName = "amount-label";

	private VisualElement _mainContainer;

	private VisualElement _iconContainer;
	private Image _iconImage;

	private VisualElement _nameContainer;
	private Label _nameLabel;

	private VisualElement _amountContainer;
	private Label _amountLabel;

	private ProductAmount _productAmount;

	public SingleItemView(VisualTreeAsset template)
    {
		var instance = template.Instantiate();
		_mainContainer = instance.Q<VisualElement>(_singleValueName);

		_iconContainer = _mainContainer.Q<VisualElement>(_iconContainerName);
		_iconImage = _iconContainer.Q<Image>(_iconImageName);

		_nameContainer = _mainContainer.Q<VisualElement>(_nameContainerName);
		_nameLabel = _nameContainer.Q<Label>(_nameLabelName);

		_amountContainer = _mainContainer.Q<VisualElement>(_amountContainerName);
		_amountLabel = _amountContainer.Q<Label>(_amountLabelName);
    }

	public VisualElement Bind(ProductAmount productAmount, bool hideIcon, bool hideName, bool hideAmount)
	{
		if(productAmount == null)
			throw new UnityException("Inventory single item view asked to bind to a null product amount");

		_productAmount = productAmount;
		_productAmount.OnAmountChanged += UpdateValues;
		UpdateValues(_productAmount);

		if(hideIcon)
			_iconContainer.style.display = DisplayStyle.None;

		if(hideName)
			_nameContainer.style.display = DisplayStyle.None;

		if(hideAmount)
			_amountContainer.style.display = DisplayStyle.None;

		return _mainContainer;
	}

	private void UpdateValues(ProductAmount productAmount)
	{
		_iconImage.sprite = _productAmount.Product.Icon;
		_iconImage.tintColor = _productAmount.Product.IconTintColor;

		_nameLabel.text = _productAmount.Product.DisplayName;
		_amountLabel.text = _productAmount.Amount.ToString();
	}
}