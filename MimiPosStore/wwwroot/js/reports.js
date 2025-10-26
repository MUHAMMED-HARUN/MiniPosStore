/**
 * Professional Reports JavaScript Module
 * Handles dynamic report generation, data visualization, and user interactions
 * Built with modern JavaScript and clean code principles
 */

class ReportsManager {
    constructor() {
        this.isLoading = false;
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.setupDateValidation();
        this.setupQuickReports();
        this.animateCounters();
    }

    setupEventListeners() {
        // Form submission handling
        $('#reportsForm').on('submit', (e) => this.handleFormSubmission(e));
        
        // Date change handlers
        $('#StartDate, #EndDate').on('change', () => this.validateDateRange());
        
        // Quick report buttons
        $('.quick-report-btn').on('click', (e) => this.handleQuickReport(e));
        
        // Export functionality (can be extended)
        $(document).on('click', '.export-btn', (e) => this.exportReport(e));
    }

    setupDateValidation() {
        const today = new Date().toISOString().split('T')[0];
        
        // Set max date to today
        $('#StartDate, #EndDate').attr('max', today);
        
        // Auto-adjust end date when start date changes
        $('#StartDate').on('change', function() {
            const startDate = $(this).val();
            const endDateInput = $('#EndDate');
            const endDate = endDateInput.val();
            
            if (startDate && endDate && startDate > endDate) {
                endDateInput.val(startDate);
            }
            
            endDateInput.attr('min', startDate);
        });
    }

    setupQuickReports() {
        $('.dropdown-item[href*="QuickReport"]').on('click', (e) => {
            e.preventDefault();
            const period = $(e.target).closest('a').attr('href').split('period=')[1];
            this.setQuickReportDates(period);
            $('#reportsForm').submit();
        });
    }

    setQuickReportDates(period) {
        const today = new Date();
        let startDate, endDate = today;

        switch (period) {
            case 'today':
                startDate = today;
                break;
            case 'week':
                startDate = new Date(today);
                startDate.setDate(today.getDate() - today.getDay());
                break;
            case 'month':
                startDate = new Date(today.getFullYear(), today.getMonth(), 1);
                break;
            default:
                return;
        }

        $('#StartDate').val(this.formatDate(startDate));
        $('#EndDate').val(this.formatDate(endDate));
    }

    formatDate(date) {
        return date.toISOString().split('T')[0];
    }

    validateDateRange() {
        const startDate = new Date($('#StartDate').val());
        const endDate = new Date($('#EndDate').val());
        const today = new Date();
        
        let isValid = true;
        let errorMessage = '';

        if (startDate > endDate) {
            isValid = false;
            errorMessage = 'تاريخ البداية لا يمكن أن يكون بعد تاريخ النهاية';
        } else if (startDate > today) {
            isValid = false;
            errorMessage = 'تاريخ البداية لا يمكن أن يكون في المستقبل';
        } else if (endDate > today) {
            isValid = false;
            errorMessage = 'تاريخ النهاية لا يمكن أن يكون في المستقبل';
        }

        this.showValidationMessage(isValid, errorMessage);
        return isValid;
    }

    showValidationMessage(isValid, message) {
        const alertContainer = $('#validation-alert');
        
        if (alertContainer.length === 0 && !isValid) {
            const alert = `
                <div id="validation-alert" class="alert alert-warning alert-dismissible fade show mt-3" role="alert">
                    <i class="fas fa-exclamation-triangle me-2"></i>
                    ${message}
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
            `;
            $('#reportsForm').append(alert);
        } else if (isValid && alertContainer.length > 0) {
            alertContainer.remove();
        }
    }

    handleFormSubmission(e) {
        if (this.isLoading) {
            e.preventDefault();
            return false;
        }

        if (!this.validateDateRange()) {
            e.preventDefault();
            return false;
        }

        this.showLoadingState();
        return true;
    }

    showLoadingState() {
        this.isLoading = true;
        const btn = $('#generateBtn');
        
        btn.addClass('loading disabled');
        btn.find('.loading-spinner').show();
        btn.find('i.fas').hide();
        
        // Add loading overlay
        const overlay = `
            <div id="loading-overlay" class="position-fixed top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center" 
                 style="background: rgba(0,0,0,0.5); z-index: 9999;">
                <div class="text-center text-white">
                    <div class="spinner-border mb-3" style="width: 3rem; height: 3rem;"></div>
                    <h5>جاري إنشاء التقارير...</h5>
                    <p>الرجاء الانتظار</p>
                </div>
            </div>
        `;
        $('body').append(overlay);
    }

    hideLoadingState() {
        this.isLoading = false;
        const btn = $('#generateBtn');
        
        btn.removeClass('loading disabled');
        btn.find('.loading-spinner').hide();
        btn.find('i.fas').show();
        
        $('#loading-overlay').remove();
    }

    animateCounters() {
        // Animate financial numbers when they appear
        $('.report-item h4').each(function(index) {
            const $this = $(this);
            const finalValue = $this.text().replace(/[^\d.-]/g, '');
            
            if (!isNaN(finalValue) && finalValue !== '') {
                $this.prop('Counter', 0).animate({
                    Counter: parseFloat(finalValue)
                }, {
                    duration: 1000 + (index * 100),
                    easing: 'swing',
                    step: function(now) {
                        const formattedValue = new Intl.NumberFormat('ar-EG', {
                            style: 'currency',
                            currency: 'EGP'
                        }).format(now);
                        $this.text(formattedValue);
                    }
                });
            }
        });
    }

    exportReport(e) {
        e.preventDefault();
        // This can be extended to export to PDF, Excel, etc.
        this.showToast('تصدير التقرير', 'سيتم إضافة ميزة التصدير قريباً', 'info');
    }

    showToast(title, message, type = 'info') {
        const toastHtml = `
            <div class="toast align-items-center text-white bg-${type} border-0" role="alert">
                <div class="d-flex">
                    <div class="toast-body">
                        <strong>${title}</strong><br>
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;
        
        let toastContainer = $('.toast-container');
        if (toastContainer.length === 0) {
            toastContainer = $('<div class="toast-container position-fixed bottom-0 end-0 p-3"></div>');
            $('body').append(toastContainer);
        }
        
        const toast = $(toastHtml);
        toastContainer.append(toast);
        
        const bsToast = new bootstrap.Toast(toast[0]);
        bsToast.show();
        
        toast.on('hidden.bs.toast', function() {
            $(this).remove();
        });
    }

    // AJAX method for real-time report updates
    async fetchReportsData(startDate, endDate) {
        try {
            const response = await fetch('/Reports/GetReportsData', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                body: JSON.stringify({
                    startDate: startDate,
                    endDate: endDate
                })
            });

            const data = await response.json();
            
            if (data.success) {
                this.updateReportsUI(data.data);
                this.showToast('نجح', 'تم تحديث التقارير بنجاح', 'success');
            } else {
                this.showToast('خطأ', data.message, 'danger');
            }
        } catch (error) {
            console.error('Error fetching reports:', error);
            this.showToast('خطأ', 'حدث خطأ في جلب البيانات', 'danger');
        }
    }

    updateReportsUI(data) {
        // Update UI with new data without page refresh
        $('#OrderSales').text(this.formatCurrency(data.orderSales));
        $('#OrderNetProfit').text(this.formatCurrency(data.orderNetProfit));
        $('#RemainingOrderDebt').text(this.formatCurrency(data.remainingOrderDebt));
        $('#ImportOrderCost').text(this.formatCurrency(data.importOrderCost));
        $('#RemainingImportDebt').text(this.formatCurrency(data.remainingImportDebt));
        $('#TotalExpenses').text(this.formatCurrency(data.totalExpenses));
        $('#StoreNetProfit').text(this.formatCurrency(data.storeNetProfit));
        
        // Re-animate counters
        this.animateCounters();
    }

    formatCurrency(amount) {
        return new Intl.NumberFormat('ar-EG', {
            style: 'currency',
            currency: 'EGP'
        }).format(amount);
    }
}

// Initialize when DOM is ready
$(document).ready(function() {
    window.reportsManager = new ReportsManager();
    
    // Global error handler for AJAX requests
    $(document).ajaxError(function(event, xhr, settings, error) {
        console.error('AJAX Error:', error);
        if (window.reportsManager) {
            window.reportsManager.hideLoadingState();
            window.reportsManager.showToast('خطأ', 'حدث خطأ في الاتصال بالخادم', 'danger');
        }
    });
});

// Export for use in other modules
window.ReportsManager = ReportsManager;
