var yandexGamesLibrary = {
    $yandexGames: {
        isInitialized: false,
        isAuthorized: false,
        isInitializeCalled: false,
        scopes: undefined,
        sdk: undefined,
        leaderboard: undefined,
        playerAccount: undefined,
        billing: undefined,
        pauseCallbackPtr: undefined,
        historyBackCallbackPtr: undefined,
        playerAuthorizedCallbackPtr: undefined,
        yandexGamesSdkInitialize: function (scopes, successCallbackPtr) {
            if (yandexGames.isInitializeCalled)
                return;
            yandexGames.isInitializeCalled = true;
            yandexGames.scopes = scopes;
            var sdkScript = document.createElement('script');
            sdkScript.src = '/sdk.js';
            document.head.appendChild(sdkScript);
            sdkScript.onload = function () {
                YaGames.init().then(function (sdk) {
                    yandexGames.sdk = sdk;
                    var pauseCallback = function () {
                        if (yandexGames.pauseCallbackPtr !== undefined)
                            dynCall('vi', yandexGames.pauseCallbackPtr, [true]);
                    };
                    var resumeCallback = function () {
                        if (!yandexGames.isAuthorized) {
                            yandexGames.sdk.getPlayer({ scopes: yandexGames.scopes })
                                .then(function (playerAccount) {
                                if (playerAccount.getMode() !== 'lite') {
                                    yandexGames.isAuthorized = true;
                                    yandexGames.playerAccount = playerAccount;
                                    if (yandexGames.playerAuthorizedCallbackPtr !== undefined)
                                        dynCall('v', yandexGames.playerAuthorizedCallbackPtr, []);
                                }
                            });
                        }
                        if (yandexGames.pauseCallbackPtr !== undefined)
                            dynCall('vi', yandexGames.pauseCallbackPtr, [false]);
                    };
                    var historyBackCallback = function () {
                        if (yandexGames.historyBackCallbackPtr !== undefined)
                            dynCall('v', yandexGames.historyBackCallbackPtr, []);
                    };
                    var playerAccountInitializationPromise = sdk.getPlayer({ scopes: yandexGames.scopes })
                        .then(function (playerAccount) {
                        if (playerAccount.getMode() !== 'lite')
                            yandexGames.isAuthorized = true;
                        yandexGames.playerAccount = playerAccount;
                    }).catch(function () {
                        throw new Error('PlayerAccount failed to initialize.');
                    });
                    var leaderboardInitializationPromise = sdk.getLeaderboards()
                        .then(function (leaderboard) {
                        yandexGames.leaderboard = leaderboard;
                    }).catch(function () {
                        throw new Error('Leaderboard failed to initialize.');
                    });
                    var billingInitializationPromise = sdk.getPayments({ signed: true })
                        .then(function (billing) {
                        yandexGames.billing = billing;
                    });
                    Promise.all([playerAccountInitializationPromise,
                        leaderboardInitializationPromise,
                        billingInitializationPromise]).then(function () {
                        yandexGames.sdk.on('game_api_pause', pauseCallback);
                        yandexGames.sdk.on('game_api_resume', resumeCallback);
                        yandexGames.sdk.on(yandexGames.sdk.EVENTS.HISTORY_BACK, historyBackCallback);
                        yandexGames.isInitialized = true;
                        {{{ makeDynCall('v', 'successCallbackPtr') }}}();
                    });
                });
            };
        },
        gameReady: function () {
            yandexGames.sdk.features.LoadingAPI.ready();
        },
        gameStart: function () {
            yandexGames.sdk.features.GameplayAPI.start();
        },
        gameStop: function () {
            yandexGames.sdk.features.GameplayAPI.stop();
        },
        playerAccountAuthorize: function (successCallbackPtr, errorCallbackPtr) {
            if (yandexGames.isAuthorized) {
                console.error('Already authorized.');
                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
                return;
            }
            yandexGames.sdk.auth.openAuthDialog().then(function () {
                yandexGames.sdk.getPlayer({ scopes: yandexGames.scopes }).then(function (playerAccount) {
                    yandexGames.isAuthorized = true;
                    yandexGames.playerAccount = playerAccount;
                    {{{ makeDynCall('v', 'successCallbackPtr') }}}();
                    if (yandexGames.playerAuthorizedCallbackPtr !== undefined)
                        dynCall('v', yandexGames.playerAuthorizedCallbackPtr, []);
                }).catch(function (error) {
                    console.error('Authorize failed to update playerAccount. Assuming authorization failed. Error was: ' + error.message);
                    yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                });
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        playerAccountSetCloudSaveData: function (cloudSaveDataJson, flush, successCallbackPtr, errorCallbackPtr) {
            var cloudSaveData;
            try {
                cloudSaveData = JSON.parse(cloudSaveDataJson);
            }
            catch (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                return;
            }
            yandexGames.playerAccount.setData(cloudSaveData, flush).then(function () {
                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        playerAccountGetCloudSaveData: function (successCallbackPtr, errorCallbackPtr) {
            var _this = this;
            yandexGames.playerAccount.getData().then(function (cloudSaveData) {
                var cloudSaveDataUnmanagedStringPtr = _this.allocateUnmanagedString(JSON.stringify(cloudSaveData));
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(cloudSaveDataUnmanagedStringPtr);
                _free(cloudSaveDataUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        playerAccountGetKeysCloudSaveData: function (keysJson, successCallbackPtr, errorCallbackPtr) {
            var _this = this;
            var keys;
            try {
                keys = JSON.parse(keysJson);
            }
            catch (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                return;
            }
            var keysRequest = keys.keys.length === 0 ? undefined : keys.keys;
            yandexGames.playerAccount.getData(keysRequest).then(function (data) {
                var keysData = {
                    keys: Object.keys(data),
                    values: Object.keys(data).map(function (key) {
                        return JSON.stringify(data[key]);
                    }),
                };
                var keysDataUnmanagedStringPtr = _this.allocateUnmanagedString(JSON.stringify(keysData));
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(keysDataUnmanagedStringPtr);
                _free(keysDataUnmanagedStringPtr);
            });
        },
        playerAccountSetStats: function (statsJson, successCallbackPtr, errorCallbackPtr) {
            var stats = this.parseStatsInput(statsJson, errorCallbackPtr);
            if (stats === null)
                return;
            yandexGames.playerAccount.setStats(stats).then(function () {
                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        playerAccountIncrementStats: function (statsJson, successCallbackPtr, errorCallbackPtr) {
            var stats = this.parseStatsInput(statsJson, errorCallbackPtr);
            if (stats === null)
                return;
            yandexGames.playerAccount.incrementStats(stats).then(function (result) {
                var incrementedStats = {
                    keys: Object.keys(result.stats),
                    values: Object.keys(result.stats).map(function (key) { return result.stats[key]; }),
                };
                var incrementedStatsUnmanagedStringPtr = yandexGames.allocateUnmanagedString(JSON.stringify(incrementedStats));
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(incrementedStatsUnmanagedStringPtr);
                _free(incrementedStatsUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        playerAccountGetStats: function (keysJson, successCallbackPtr, errorCallbackPtr) {
            var input = this.parseStatsInput(keysJson, errorCallbackPtr);
            var keys = input === null ? undefined : Object.keys(input).length === 0 ? undefined : Object.keys(input);
            yandexGames.playerAccount.getStats(keys).then(function (result) {
                var stats = {
                    keys: Object.keys(result),
                    values: Object.keys(result).map(function (key) { return result[key] !== undefined ? result[key] : 0; }),
                };
                var statsUnmanagedStringPtr = yandexGames.allocateUnmanagedString(JSON.stringify(stats));
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(statsUnmanagedStringPtr);
                _free(statsUnmanagedStringPtr);
            });
        },
        parseStatsInput: function (statsJson, errorCallbackPtr) {
            var stats;
            try {
                stats = JSON.parse(statsJson);
                return stats.keys.reduce(function (acc, key, index) {
                    acc[key] = stats.values[index];
                    return acc;
                }, {});
            }
            catch (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                return null;
            }
        },
        playerAccountGetProfileData: function (successCallbackPtr, errorCallbackPtr, pictureSize) {
            var _this = this;
            yandexGames.sdk.getPlayer({ scopes: yandexGames.scopes }).then(function (playerAccount) {
                yandexGames.playerAccount = playerAccount;
                var personalInfo = {
                    uniqueID: playerAccount.getUniqueID(),
                    name: playerAccount.getName(),
                    profilePicture: playerAccount.getPhoto(pictureSize),
                    payingStatus: playerAccount.getPayingStatus(),
                    userIDsPerGame: undefined,
                };
                playerAccount.getIDsPerGame().then(function (ids) {
                    personalInfo.userIDsPerGame = ids;
                    var profileDataJson = JSON.stringify(personalInfo);
                    var profileDataUnmanagedStringPtr = _this.allocateUnmanagedString(profileDataJson);
                    {{{ makeDynCall('vi', 'successCallbackPtr') }}}(profileDataUnmanagedStringPtr);
                    _free(profileDataUnmanagedStringPtr);
                });
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        remoteConfigurationGetFlags: function (defaultFlagsJson, clientFeaturesJson, resultCallbackPtr, errorCallbackPtr) {
            var defaultFlags;
            try {
                defaultFlags = JSON.parse(defaultFlagsJson);
            }
            catch (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                return;
            }
            var clientFeatures;
            try {
                clientFeatures = JSON.parse(clientFeaturesJson);
            }
            catch (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                return;
            }
            var params = {
                defaultFlags: defaultFlags.keys.reduce(function (flag, key, i) {
                    flag[key] = defaultFlags.values[i];
                    return flag;
                }, {}),
                clientFeatures: clientFeatures.features,
            };
            yandexGames.sdk.getFlags(params).then(function (flags) {
                defaultFlags = {
                    keys: Object.keys(flags),
                    values: Object.keys(flags).map(function (key) { return flags[key]; }),
                };
                var flagsUnmanagedStringPtr = yandexGames.allocateUnmanagedString(JSON.stringify(defaultFlags));
                {{{ makeDynCall('vi', 'resultCallbackPtr') }}}(flagsUnmanagedStringPtr);
                _free(flagsUnmanagedStringPtr);
            });
        },
        interstitialAdShow: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr) {
            yandexGames.sdk.adv.showFullscreenAdv({
                callbacks: {
                    onOpen: function () {
                        {{{ makeDynCall('v', 'openCallbackPtr') }}}();
                    },
                    onClose: function (wasShown) {
                        {{{ makeDynCall('vi', 'closeCallbackPtr') }}}(wasShown);
                    },
                    onError: function (error) {
                        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                    },
                    onOffline: function () {
                        {{{ makeDynCall('v', 'offlineCallbackPtr') }}}();
                    },
                }
            });
        },
        rewardedAdShow: function (openCallbackPtr, rewardedCallbackPtr, closeCallbackPtr, errorCallbackPtr) {
            yandexGames.sdk.adv.showRewardedVideo({
                callbacks: {
                    onOpen: function () {
                        {{{ makeDynCall('v', 'openCallbackPtr') }}}();
                    },
                    onRewarded: function () {
                        {{{ makeDynCall('v', 'rewardedCallbackPtr') }}}();
                    },
                    onClose: function () {
                        {{{ makeDynCall('v', 'closeCallbackPtr') }}}();
                    },
                    onError: function (error) {
                        yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                    },
                }
            });
        },
        stickyAdGetStatus: function (resultCallbackPtr) {
            yandexGames.sdk.adv.getBannerAdvStatus().then(function (status) {
                var reason = status.reason === undefined ? 'No reason' : status.reason;
                var reasonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(reason);
                {{{ makeDynCall('vii', 'resultCallbackPtr') }}}(status.stickyAdvIsShowing, reasonUnmanagedStringPtr);
            });
        },
        stickyAdShow: function () {
            yandexGames.sdk.adv.showBannerAdv();
        },
        stickyAdHide: function () {
            yandexGames.sdk.adv.hideBannerAdv();
        },
        billingPurchaseProduct: function (productId, successCallbackPtr, errorCallbackPtr, developerPayload) {
            yandexGames.billing.purchase({ id: productId, developerPayload: developerPayload }).then(function (purchaseResponse) {
                var purchasedProductJson = JSON.stringify(purchaseResponse);
                var purchasedProductJsonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(purchasedProductJson);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(purchasedProductJsonUnmanagedStringPtr);
                _free(purchasedProductJsonUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        billingGetPurchasedProducts: function (successCallbackPtr, errorCallbackPtr) {
            yandexGames.billing.getPurchases().then(function (purchasesResponse) {
                var purchasedProductsJson = JSON.stringify({ products: purchasesResponse, signature: purchasesResponse.signature });
                var purchasedProductsJsonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(purchasedProductsJson);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(purchasedProductsJsonUnmanagedStringPtr);
                _free(purchasedProductsJsonUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        billingGetCatalogProducts: function (successCallbackPtr, errorCallbackPtr, currencyPictureSize) {
            yandexGames.billing.getCatalog().then(function (productCatalogResponse) {
                var products = [];
                for (var catalogIterator = 0; catalogIterator < productCatalogResponse.length; catalogIterator++) {
                    products[catalogIterator] = {
                        id: productCatalogResponse[catalogIterator].id,
                        title: productCatalogResponse[catalogIterator].title,
                        description: productCatalogResponse[catalogIterator].description,
                        imageURI: productCatalogResponse[catalogIterator].imageURI,
                        price: productCatalogResponse[catalogIterator].price,
                        priceValue: productCatalogResponse[catalogIterator].priceValue,
                        priceCurrencyCode: productCatalogResponse[catalogIterator].priceCurrencyCode,
                        priceCurrencyImage: "https:" + productCatalogResponse[catalogIterator].getPriceCurrencyImage(currencyPictureSize),
                    };
                }
                var productCatalogJson = JSON.stringify({ products: products });
                var productCatalogJsonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(productCatalogJson);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(productCatalogJsonUnmanagedStringPtr);
                _free(productCatalogJsonUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        billingConsumeProduct: function (purchasedProductToken, successCallbackPtr, errorCallbackPtr) {
            yandexGames.billing.consumePurchase(purchasedProductToken).then(function () {
                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        leaderboardGetDescription: function (leaderboardName, successCallbackPtr, errorCallbackPtr) {
            var _this = this;
            yandexGames.leaderboard.getLeaderboardDescription(leaderboardName)
                .then(function (leaderboard) {
                var description = {
                    appID: leaderboard.appID,
                    default: leaderboard.default,
                    description: {
                        invert_sort_order: leaderboard.description.invert_sort_order,
                        score_format: {
                            options: {
                                decimal_offset: leaderboard.description.score_format.options.decimal_offset,
                            },
                        },
                        type: leaderboard.description.type,
                    },
                    technicalName: leaderboard.name,
                    title: Object.keys(leaderboard.title).map(function (locale) { return ({ locale: locale, name: leaderboard.title[locale] }); }),
                };
                var descriptionJson = JSON.stringify(description);
                var descriptionUnmanagedStringPtr = _this.allocateUnmanagedString(descriptionJson);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(descriptionUnmanagedStringPtr);
                _free(descriptionUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        leaderboardSetScore: function (leaderboardName, score, successCallbackPtr, errorCallbackPtr, extraData) {
            var _this = this;
            if (this.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
                console.error('leaderboardSetScore requires authorization.');
                return;
            }
            yandexGames.leaderboard.setLeaderboardScore(leaderboardName, score, extraData).then(function () {
                {{{ makeDynCall('v', 'successCallbackPtr') }}}();
            }).catch(function (error) {
                _this.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        leaderboardGetPlayerEntry: function (leaderboardName, successCallbackPtr, errorCallbackPtr, pictureSize) {
            var _this = this;
            if (this.invokeErrorCallbackIfNotAuthorized(errorCallbackPtr)) {
                console.error('leaderboardGetPlayerEntry requires authorization.');
                return;
            }
            yandexGames.leaderboard.getLeaderboardPlayerEntry(leaderboardName).then(function (response) {
                var entry = {
                    score: response.score,
                    extraData: response.extraData,
                    rank: response.rank,
                    player: {
                        profilePicture: response.player.getAvatarSrc(pictureSize),
                        profilePictureSrcSet: response.player.getAvatarSrcSet(pictureSize),
                        lang: response.player.lang,
                        publicName: response.player.publicName,
                        scopePermissions: {
                            avatar: response.player.scopePermissions.avatar,
                            public_Name: response.player.scopePermissions.public_name,
                        },
                        uniqueID: response.player.uniqueID,
                    },
                    formattedScore: response.formattedScore,
                };
                var entryJson = JSON.stringify(entry);
                var entryJsonUnmanagedStringPtr = _this.allocateUnmanagedString(entryJson);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(entryJsonUnmanagedStringPtr);
                _free(entryJsonUnmanagedStringPtr);
            }).catch(function (error) {
                if (error.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
                    var nullUnmanagedStringPtr = yandexGames.allocateUnmanagedString('null');
                    {{{ makeDynCall('vi', 'successCallbackPtr') }}}(nullUnmanagedStringPtr);
                    _free(nullUnmanagedStringPtr);
                }
                else {
                    yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                }
            });
        },
        leaderboardGetEntries: function (leaderboardName, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, includeSelf, pictureSize) {
            if (!yandexGames.isAuthorized)
                includeSelf = false;
            yandexGames.leaderboard.getLeaderboardEntries(leaderboardName, {
                includeUser: includeSelf, quantityAround: competingPlayersCount, quantityTop: topPlayersCount
            }).then(function (response) {
                var leaderboard = {
                    appID: response.leaderboard.appID,
                    default: response.leaderboard.default,
                    description: {
                        invert_sort_order: response.leaderboard.description.invert_sort_order,
                        score_format: {
                            options: {
                                decimal_offset: response.leaderboard.description.score_format.options.decimal_offset,
                            },
                        },
                        type: response.leaderboard.description.type,
                    },
                    technicalName: response.leaderboard.name,
                    title: Object.keys(response.leaderboard.title).map(function (locale) { return ({ locale: locale, name: response.leaderboard.title[locale] }); }),
                };
                var leaderboardEntries = {
                    leaderboard: leaderboard,
                    ranges: response.ranges,
                    userRank: response.userRank,
                    entries: response.entries.map(function (entry) {
                        return ({
                            score: entry.score,
                            extraData: entry.extraData,
                            rank: entry.rank,
                            player: {
                                profilePicture: entry.player.getAvatarSrc(pictureSize),
                                profilePictureSrcSet: entry.player.getAvatarSrcSet(pictureSize),
                                lang: entry.player.lang,
                                publicName: entry.player.publicName,
                                scopePermissions: {
                                    avatar: entry.player.scopePermissions.avatar,
                                    public_name: entry.player.scopePermissions.public_name,
                                },
                                uniqueID: entry.player.uniqueID,
                            },
                            formattedScore: entry.formattedScore,
                        });
                    }),
                };
                var entriesJson = JSON.stringify(leaderboardEntries);
                var entriesUnmanagedStringPtr = yandexGames.allocateUnmanagedString(entriesJson);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(entriesUnmanagedStringPtr);
                _free(entriesUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        feedbackCanReview: function (resultCallbackPtr) {
            yandexGames.sdk.feedback.canReview().then(function (result) {
                var reason = result.reason === undefined ? 'No reason' : result.reason;
                var reasonUnmanagedStringPtr = yandexGames.allocateUnmanagedString(reason);
                {{{ makeDynCall('vii', 'resultCallbackPtr') }}}(result.value, reasonUnmanagedStringPtr);
                _free(reasonUnmanagedStringPtr);
            });
        },
        feedbackRequestReview: function (resultCallbackPtr) {
            yandexGames.sdk.feedback.requestReview().then(function (result) {
                {{{ makeDynCall('vi', 'resultCallbackPtr') }}}(result);
            });
        },
        shortcutCanShowPrompt: function (resultCallbackPtr) {
            yandexGames.sdk.shortcut.canShowPrompt().then(function (prompt) {
                {{{ makeDynCall('vi', 'resultCallbackPtr') }}}(prompt.canShow);
            });
        },
        shortcutShowPrompt: function (resultCallbackPtr) {
            yandexGames.sdk.shortcut.showPrompt().then(function (result) {
                {{{ makeDynCall('vi', 'resultCallbackPtr') }}}(result.outcome === 'accepted');
            });
        },
        getYandexGamesSdkEnvironment: function () {
            var environmentJson = JSON.stringify(yandexGames.sdk.environment);
            return yandexGames.allocateUnmanagedString(environmentJson);
        },
        getServerTime: function () {
            var serverTime = yandexGames.sdk.serverTime().toString();
            return yandexGames.allocateUnmanagedString(serverTime);
        },
        gamesAPIGetAllGames: function (successCallbackPtr, errorCallbackPtr) {
            yandexGames.sdk.features.GamesAPI.getAllGames().then(function (response) {
                var responseJson = JSON.stringify(response);
                var responseUnmanagedStringPtr = yandexGames.allocateUnmanagedString(responseJson);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(responseUnmanagedStringPtr);
                _free(responseUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        gamesAPIGetGameByID: function (gameId, successCallbackPtr, errorCallbackPtr) {
            yandexGames.sdk.features.GamesAPI.getGameByID(gameId).then(function (response) {
                var responseJson = JSON.stringify(response);
                var responseUnmanagedStringPtr = yandexGames.allocateUnmanagedString(responseJson);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(responseUnmanagedStringPtr);
                _free(responseUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        screenIsFullscreen: function () {
            return yandexGames.sdk.screen.fullscreen.status === 'on';
        },
        setFullscreen: function (isFullscreen, successCallbackPtr, errorCallbackPtr) {
            if (isFullscreen === yandexGames.screenIsFullscreen())
                return;
            if (isFullscreen)
                yandexGames.sdk.screen.fullscreen.request().then(function () {
                    {{{ makeDynCall('vi', 'successCallbackPtr') }}}(isFullscreen);
                }).catch(function (error) {
                    yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                });
            else
                yandexGames.sdk.screen.fullscreen.exit().then(function () {
                    {{{ makeDynCall('vi', 'successCallbackPtr') }}}(isFullscreen);
                }).catch(function (error) {
                    yandexGames.invokeErrorCallback(error, errorCallbackPtr);
                });
        },
        clipboardWrite: function (clipboardText) {
            yandexGames.sdk.clipboard.writeText(clipboardText);
        },
        clipboardRead: function (successCallbackPtr, errorCallbackPtr) {
            navigator.clipboard.readText().then(function (clipboardText) {
                var clipboardTextUnmanagedStringPtr = yandexGames.allocateUnmanagedString(clipboardText);
                {{{ makeDynCall('vi', 'successCallbackPtr') }}}(clipboardTextUnmanagedStringPtr);
                _free(clipboardTextUnmanagedStringPtr);
            }).catch(function (error) {
                yandexGames.invokeErrorCallback(error, errorCallbackPtr);
            });
        },
        getDeviceInfo: function () {
            var deviceInfo = {
                deviceType: yandexGames.sdk.deviceInfo.type,
                isMobile: yandexGames.sdk.deviceInfo.isMobile(),
                isTablet: yandexGames.sdk.deviceInfo.isTablet(),
                isDesktop: yandexGames.sdk.deviceInfo.isDesktop(),
                isTV: yandexGames.sdk.deviceInfo.isTV(),
            };
            var deviceInfoJson = JSON.stringify(deviceInfo);
            return yandexGames.allocateUnmanagedString(deviceInfoJson);
        },
        throwIfSdkNotInitialized: function () {
            if (!this.isInitialized)
                throw new Error('SDK is not initialized. Invoke YandexGamesSdk.Initialize() coroutine and wait for it to finish.');
        },
        invokeErrorCallback: function (error, errorCallbackPtr) {
            var errorMessage;
            if (error instanceof Error) {
                errorMessage = error.message;
                if (errorMessage === null) {
                    errorMessage = 'SDK API thrown an error with null message.';
                }
                if (errorMessage === undefined) {
                    errorMessage = 'SDK API thrown an error with undefined message.';
                }
            }
            else if (typeof error === 'string') {
                errorMessage = error;
            }
            else if (error) {
                errorMessage = 'SDK API thrown an unexpected type as error: ' + JSON.stringify(error);
            }
            else if (error === null) {
                errorMessage = 'SDK API thrown a null as error.';
            }
            else {
                errorMessage = 'SDK API thrown an undefined as error.';
            }
            var errorUnmanagedStringPtr = this.allocateUnmanagedString(errorMessage);
            {{{ makeDynCall('vi', 'errorCallbackPtr') }}}(errorUnmanagedStringPtr);
            _free(errorUnmanagedStringPtr);
        },
        invokeErrorCallbackIfNotAuthorized: function (errorCallbackPtr) {
            if (!yandexGames.isAuthorized) {
                this.invokeErrorCallback(new Error('Needs authorization.'), errorCallbackPtr);
                return true;
            }
            return false;
        },
        allocateUnmanagedString: function (string) {
            var stringBufferSize = lengthBytesUTF8(string) + 1;
            var stringBufferPtr = _malloc(stringBufferSize);
            stringToUTF8(string, stringBufferPtr, stringBufferSize);
            return stringBufferPtr;
        },
    },
    YandexGamesSdkInitialize: function (scopes, successCallbackPtr) {
        yandexGames.yandexGamesSdkInitialize(!!scopes, successCallbackPtr);
    },
    GetYandexGamesSdkIsInitialized: function () {
        return yandexGames.isInitialized;
    },
    YandexGamesSdkGameReady: function () {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.gameReady();
    },
    YandexGamesSdkGameStart: function () {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.gameStart();
    },
    YandexGamesSdkGameStop: function () {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.gameStop();
    },
    AddGamePauseListener: function (gamePausedCallbackPtr) {
        yandexGames.pauseCallbackPtr = gamePausedCallbackPtr;
    },
    PlayerAccountAuthorize: function (successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.playerAccountAuthorize(successCallbackPtr, errorCallbackPtr);
    },
    GetPlayerAccountIsAuthorized: function () {
        yandexGames.throwIfSdkNotInitialized();
        return yandexGames.isAuthorized;
    },
    AddPlayerAuthorizationListener: function (playerAuthorizedCallbackPtr) {
        yandexGames.playerAuthorizedCallbackPtr = playerAuthorizedCallbackPtr;
    },
    AddHistoryBackEventListener: function (historyCallbackPtr) {
        yandexGames.historyBackCallbackPtr = historyCallbackPtr;
    },
    DispatchExitEvent: function () {
        yandexGames.sdk.dispatchEvent(yandexGames.sdk.EVENTS.EXIT);
    },
    PlayerAccountSetCloudSaveData: function (cloudSaveDataJsonPtr, flush, successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var cloudSaveDataJson = UTF8ToString(cloudSaveDataJsonPtr);
        yandexGames.playerAccountSetCloudSaveData(cloudSaveDataJson, !!flush, successCallbackPtr, errorCallbackPtr);
    },
    PlayerAccountGetCloudSaveData: function (successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.playerAccountGetCloudSaveData(successCallbackPtr, errorCallbackPtr);
    },
    PlayerAccountGetKeysCloudSaveData: function (keysJsonPtr, successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var keysJson = UTF8ToString(keysJsonPtr);
        yandexGames.playerAccountGetKeysCloudSaveData(keysJson, successCallbackPtr, errorCallbackPtr);
    },
    PlayerAccountSetStats: function (statsJsonPtr, successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var statsJson = UTF8ToString(statsJsonPtr);
        yandexGames.playerAccountSetStats(statsJson, successCallbackPtr, errorCallbackPtr);
    },
    PlayerAccountIncrementStats: function (statsJsonPtr, successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var statsJson = UTF8ToString(statsJsonPtr);
        yandexGames.playerAccountIncrementStats(statsJson, successCallbackPtr, errorCallbackPtr);
    },
    PlayerAccountGetStats: function (keysJsonPtr, successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var keysJson = UTF8ToString(keysJsonPtr);
        yandexGames.playerAccountGetStats(keysJson, successCallbackPtr, errorCallbackPtr);
    },
    PlayerAccountGetProfileData: function (successCallbackPtr, errorCallbackPtr, pictureSizePtr) {
        yandexGames.throwIfSdkNotInitialized();
        var pictureSize = UTF8ToString(pictureSizePtr);
        yandexGames.playerAccountGetProfileData(successCallbackPtr, errorCallbackPtr, pictureSize);
    },
    RemoteConfigurationGetFlags: function (defaultFlagsJsonPtr, clientFeaturesJsonPtr, resultCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var defaultFlagsJson = UTF8ToString(defaultFlagsJsonPtr);
        var clientFeaturesJson = UTF8ToString(clientFeaturesJsonPtr);
        yandexGames.remoteConfigurationGetFlags(defaultFlagsJson, clientFeaturesJson, resultCallbackPtr, errorCallbackPtr);
    },
    InterstitialAdShow: function (openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.interstitialAdShow(openCallbackPtr, closeCallbackPtr, errorCallbackPtr, offlineCallbackPtr);
    },
    RewardedAdShow: function (openCallbackPtr, rewardedCallback, closeCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.rewardedAdShow(openCallbackPtr, rewardedCallback, closeCallbackPtr, errorCallbackPtr);
    },
    StickyAdGetStatus: function (resultCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.stickyAdGetStatus(resultCallbackPtr);
    },
    StickyAdShow: function () {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.stickyAdShow();
    },
    StickyAdHide: function () {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.stickyAdHide();
    },
    BillingPurchaseProduct: function (productIdPtr, successCallbackPtr, errorCallbackPtr, developerPayloadPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var productId = UTF8ToString(productIdPtr);
        var developerPayload = UTF8ToString(developerPayloadPtr);
        if (developerPayload.length === 0) {
            developerPayload = undefined;
        }
        yandexGames.billingPurchaseProduct(productId, successCallbackPtr, errorCallbackPtr, developerPayload);
    },
    BillingGetPurchasedProducts: function (successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.billingGetPurchasedProducts(successCallbackPtr, errorCallbackPtr);
    },
    BillingGetCatalogProducts: function (successCallbackPtr, errorCallbackPtr, currencyPictureSizePtr) {
        yandexGames.throwIfSdkNotInitialized();
        var currencyPictureSize = UTF8ToString(currencyPictureSizePtr);
        yandexGames.billingGetCatalogProducts(successCallbackPtr, errorCallbackPtr, currencyPictureSize);
    },
    BillingConsumeProduct: function (purchasedProductTokenPtr, successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var purchasedProductToken = UTF8ToString(purchasedProductTokenPtr);
        yandexGames.billingConsumeProduct(purchasedProductToken, successCallbackPtr, errorCallbackPtr);
    },
    LeaderboardGetDescription: function (leaderboardNamePtr, successCallbackPtr, errorCallbackPtr) {
        var leaderboardName = UTF8ToString(leaderboardNamePtr);
        yandexGames.leaderboardGetDescription(leaderboardName, successCallbackPtr, errorCallbackPtr);
    },
    LeaderboardSetScore: function (leaderboardNamePtr, score, successCallbackPtr, errorCallbackPtr, extraDataPtr) {
        yandexGames.throwIfSdkNotInitialized();
        var leaderboardName = UTF8ToString(leaderboardNamePtr);
        var extraData = UTF8ToString(extraDataPtr);
        if (extraData.length === 0) {
            extraData = undefined;
        }
        yandexGames.leaderboardSetScore(leaderboardName, score, successCallbackPtr, errorCallbackPtr, extraData);
    },
    LeaderboardGetPlayerEntry: function (leaderboardNamePtr, successCallbackPtr, errorCallbackPtr, pictureSizePtr) {
        yandexGames.throwIfSdkNotInitialized();
        var leaderboardName = UTF8ToString(leaderboardNamePtr);
        var pictureSize = UTF8ToString(pictureSizePtr);
        yandexGames.leaderboardGetPlayerEntry(leaderboardName, successCallbackPtr, errorCallbackPtr, pictureSize);
    },
    LeaderboardGetEntries: function (leaderboardNamePtr, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, includeSelf, pictureSizePtr) {
        yandexGames.throwIfSdkNotInitialized();
        var leaderboardName = UTF8ToString(leaderboardNamePtr);
        var pictureSize = UTF8ToString(pictureSizePtr);
        yandexGames.leaderboardGetEntries(leaderboardName, successCallbackPtr, errorCallbackPtr, topPlayersCount, competingPlayersCount, !!includeSelf, pictureSize);
    },
    FeedbackCanReview: function (resultCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.feedbackCanReview(resultCallbackPtr);
    },
    FeedbackRequestReview: function (resultCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.feedbackRequestReview(resultCallbackPtr);
    },
    ShortcutCanShowPrompt: function (resultCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.shortcutCanShowPrompt(resultCallbackPtr);
    },
    ShortcutShowPrompt: function (resultCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.shortcutShowPrompt(resultCallbackPtr);
    },
    GetYandexGamesSdkEnvironment: function () {
        yandexGames.throwIfSdkNotInitialized();
        return yandexGames.getYandexGamesSdkEnvironment();
    },
    ServerTimeGetTime: function () {
        yandexGames.throwIfSdkNotInitialized();
        return yandexGames.getServerTime();
    },
    GamesAPIGetAllGames: function (successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.gamesAPIGetAllGames(successCallbackPtr, errorCallbackPtr);
    },
    GamesAPIGetGameByID: function (gameId, successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.gamesAPIGetGameByID(gameId, successCallbackPtr, errorCallbackPtr);
    },
    ScreenIsFullScreen: function () {
        yandexGames.throwIfSdkNotInitialized();
        return yandexGames.screenIsFullscreen();
    },
    ScreenSetFullscreen: function (isFullscreen, successCallbackPtr, errorCallbackPtr) {
        yandexGames.throwIfSdkNotInitialized();
        yandexGames.setFullscreen(!!isFullscreen, successCallbackPtr, errorCallbackPtr);
    },
    ClipboardWrite: function (clipboardTextPtr) {
        var clipboardText = UTF8ToString(clipboardTextPtr);
        yandexGames.clipboardWrite(clipboardText);
    },
    ClipboardRead: function (successCallbackPtr, errorCallbackPtr) {
        yandexGames.clipboardRead(successCallbackPtr, errorCallbackPtr);
    },
    GetDeviceInfo: function () {
        yandexGames.throwIfSdkNotInitialized();
        return yandexGames.getDeviceInfo();
    },
    GamesAPIOpenLink: function (urlPtr) {
        var url = UTF8ToString(urlPtr);
        window.open(url);
    }
};
autoAddDeps(yandexGamesLibrary, '$yandexGames');
mergeInto(LibraryManager.library, yandexGamesLibrary);
