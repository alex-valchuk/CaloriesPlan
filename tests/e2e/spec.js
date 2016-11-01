describe('Protractor Demo App', function () {

    it('should have a title', function () {
        browser.get('http://localhost:3419/index.html#/login');
        expect(browser.getTitle()).toEqual('Login');
    });

    it("login to the page ", function () {
        var login = element(by.model("loginModel.userName"));
        login.sendkeys("Alex");

        var password = element(by.model("loginModel.password"));
        password.sendkeys("123456");

        var submitButton = element(by.id("btn-signin"));
        submitButton.click();

        
    });

});