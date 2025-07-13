$(document).ready(function() {
    // Thêm vào giỏ hàng
    $('.add-to-cart').on('click', function(e) {
        e.preventDefault();
        
        var productId = $(this).data('product-id');
        var quantity = parseInt($('#quantity-' + productId).val()) || 1;
        
        $.post('/Customer/Cart/AddToCart', {
            productId: productId,
            quantity: quantity
        })
        .done(function(response) {
            if (response.success) {
                // Cập nhật số lượng giỏ hàng
                updateCartCount(response.cartCount);
                
                // Hiển thị thông báo thành công
                showNotification('Đã thêm vào giỏ hàng!', 'success');
                
                // Cập nhật nút nếu cần
                $('.add-to-cart[data-product-id="' + productId + '"]')
                    .removeClass('btn-primary')
                    .addClass('btn-success')
                    .html('<i class="fa fa-check me-2"></i>Đã thêm');
                
                setTimeout(function() {
                    $('.add-to-cart[data-product-id="' + productId + '"]')
                        .removeClass('btn-success')
                        .addClass('btn-primary')
                        .html('<i class="fa fa-shopping-cart me-2"></i>Thêm vào giỏ');
                }, 2000);
            } else {
                showNotification(response.message, 'error');
            }
        })
        .fail(function() {
            showNotification('Có lỗi xảy ra khi thêm vào giỏ hàng', 'error');
        });
    });
    
    // Xử lý nút tăng/giảm số lượng
    $('.quantity-btn').on('click', function() {
        var productId = $(this).data('product-id');
        var action = $(this).data('action');
        var input = $('#quantity-' + productId);
        var currentQty = parseInt(input.val()) || 1;
        var maxQty = parseInt(input.attr('max')) || 999;
        
        if (action === 'increase' && currentQty < maxQty) {
            input.val(currentQty + 1);
        } else if (action === 'decrease' && currentQty > 1) {
            input.val(currentQty - 1);
        }
    });
    
    // Xử lý input số lượng
    $('input[id^="quantity-"]').on('change', function() {
        var value = parseInt($(this).val()) || 1;
        var max = parseInt($(this).attr('max')) || 999;
        var min = parseInt($(this).attr('min')) || 1;
        
        if (value < min) {
            $(this).val(min);
        } else if (value > max) {
            $(this).val(max);
        }
    });
    
    // Cập nhật số lượng giỏ hàng
    function updateCartCount(count) {
        var cartCount = $('#cartCount');
        if (count > 0) {
            cartCount.text(count).show();
        } else {
            cartCount.hide();
        }
    }
    
    // Hiển thị thông báo
    function showNotification(message, type) {
        var alertClass = type === 'success' ? 'alert-success' : 'alert-danger';
        var icon = type === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle';
        
        var notification = $('<div class="alert ' + alertClass + ' alert-dismissible fade show position-fixed" style="top: 20px; right: 20px; z-index: 9999; min-width: 300px;">' +
            '<i class="fa ' + icon + ' me-2"></i>' + message +
            '<button type="button" class="btn-close" data-bs-dismiss="alert"></button>' +
            '</div>');
        
        $('body').append(notification);
        
        // Tự động ẩn sau 3 giây
        setTimeout(function() {
            notification.alert('close');
        }, 3000);
    }
    
    // Load số lượng giỏ hàng khi trang load
    loadCartCount();
    
    function loadCartCount() {
        $.get('/Customer/Cart/GetCartCount')
            .done(function(count) {
                updateCartCount(count);
            })
            .fail(function() {
                // Nếu không có API, ẩn badge
                $('#cartCount').hide();
            });
    }
}); 