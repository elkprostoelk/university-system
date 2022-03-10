import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = localStorage.getItem('jwt');
        if (token) {
            const modifiedRequest = req.clone({
              headers: req.headers.set('authorization', `Bearer ${token}`)
            });
            return next.handle(modifiedRequest);
        }
        return next.handle(req);
    }

}