/*namespace training_management_internship.wwwroot.swagger
{
    public class customSwagger
    {
        window.onload = function () {
            const token = localStorage.getItem('token'); 

            if (token) {
                const swaggerUi = SwaggerUIBundle({
                    url: "/swagger/v1/swagger.json",
                    dom_id: '#swagger-ui',
                    deepLinking: true,
                    presets: [
                        SwaggerUIBundle.presets.apis,
                        SwaggerUIStandalonePreset
                    ],
                    layout: "BaseLayout",
                    requestInterceptor: (req) => {
                        req.headers['Authorization'] = 'Bearer ' + token;
                        return req;
                    }
                });
            }
    }
}
};
*/