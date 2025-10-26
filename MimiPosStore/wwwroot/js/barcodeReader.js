//// barcodeReader.js
//// هذه الدالة تقوم بإدارة كاميرا المسح ووضع النتيجة في أي عنصر تحدده
//function startBarcodeScan(videoId, outputElementId, scanButtonId) {
//    if (!window.ZXing) {
//        console.error("ZXing library is not loaded!");
//        return;
//    }

//    const codeReader = new ZXing.BrowserMultiFormatReader();

//    document.getElementById(scanButtonId).addEventListener('click', () => {
//        codeReader.decodeFromVideoDevice(null, videoId, (result, err) => {
//            if (result) {
//                // ضع النص المقروء في العنصر المحدد
//                document.getElementById(outputElementId).value = result.text;
//                codeReader.reset(); // إيقاف المسح بعد القراءة
//            }
//            if (err && !(err instanceof ZXing.NotFoundException)) {
//                console.error(err);
//            }
//        });
//    });
//}

//function setupSignalRConnection(hubUrl, receiveMethodName, onMessageReceived) {
//    if (!window.signalR) {
//        console.error("SignalR library is not loaded!");
//        return null;
//    }

//    const connection = new signalR.HubConnectionBuilder()
//        .withUrl(hubUrl)
//        .build();

//    // عند استقبال رسالة من السيرفر
//    connection.on(receiveMethodName, function (...args) {
//        if (typeof onMessageReceived === 'function') {
//            onMessageReceived(...args);
//        }
//    });

//    // بدء الاتصال
//    connection.start()
//        .then(() => console.log(`✅ Connected to Hub: ${hubUrl}`))
//        .catch(err => console.error("❌ Connection error:", err.toString()));

//    return connection;
//}


//// دالة مرنة لإرسال رسائل
//function sendSignalRMessage(connection, methodName, ...args) {
//    if (!connection) {
//        console.error("SignalR connection is not initialized.");
//        return;
//    }
//    connection.invoke(methodName, ...args)
//        .catch(err => console.error("❌ Invoke error:", err.toString()));
//}
window.startBarcodeScan = function (videoId, outputElementId, scanButtonId, onScan) {
    if (!window.ZXing) {
        console.error("ZXing library is not loaded!");
        return;
    }

    const codeReader = new ZXing.BrowserMultiFormatReader();

    document.getElementById(scanButtonId).addEventListener('click', () => {
        codeReader.decodeFromVideoDevice(null, videoId, (result, err) => {
            if (result) {
                const scannedText = result.text;
                document.getElementById(outputElementId).value = scannedText;
                codeReader.reset(); // إيقاف المسح بعد القراءة

                if (typeof onScan === "function") {
                    onScan(scannedText); // إرسال الكود
                }
            }
            if (err && !(err instanceof ZXing.NotFoundException)) {
                console.error(err);
            }
        });
    });
}

window.setupSignalRConnection = function (hubUrl, receiveMethodName, onMessageReceived) {
    if (!window.signalR) {
        console.error("SignalR library is not loaded!");
        return null;
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .build();

    connection.on(receiveMethodName, function (...args) {
        if (typeof onMessageReceived === "function") {
            onMessageReceived(...args);
        }
    });

    // ابدأ الاتصال واحتفظ بالوعد حتى تنتظره عمليات الإرسال
    connection._ready = connection.start()
        .then(() => console.log(`✅ Connected to Hub: ${hubUrl}`))
        .catch(err => {
            console.error("❌ Connection error:", err.toString());
            throw err;
        });

    return connection;
}

window.sendSignalRMessage = function (connection, methodName, ...args) {
    if (!connection) {
        const err = new Error("SignalR connection is not initialized.");
        console.error(err.message);
        return Promise.reject(err);
    }

    const waitUntilReady = connection._ready instanceof Promise
        ? connection._ready
        : Promise.resolve();

    return waitUntilReady
        .then(() => connection.invoke(methodName, ...args))
        .catch(err => {
            console.error("❌ Invoke error:", err.toString());
            throw err;
        });
}
