import { HttpInterceptorFn } from '@angular/common/http';

function getStoredToken(): string | null {
  try {
    const storage = (globalThis as unknown as { localStorage?: Storage }).localStorage;
    return storage?.getItem('token') ?? null;
  } catch {
    return null;
  }
}

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = getStoredToken();

  const authReq = req.clone({
    withCredentials: true,
    setHeaders: token ? { Authorization: `Bearer ${token}` } : {}
  });

  return next(authReq);
};
