import { useAuth } from "./hooks/useAuth";
import {useEffect, useState} from "react";
import {LoadingScreen} from "./components/common/LoadingScreen.tsx";
import OfflinePage from "./pages/OfflinePage.tsx";
import {AppRoutes} from "./routes/AppRoutes.tsx";
import Footer from "./components/common/Footer.tsx";
import {NavbarSelector} from "./components/navbar/NavbarSelector.tsx";
import { BrowserRouter } from "react-router";
import {AuthProvider} from "./context/AuthContext.tsx";
import ScrollToTop from "./components/common/ScrollToTop.tsx";

function AppContent() {
    const { loading } = useAuth();
    const [isServerDown, setIsServerDown] = useState(false);

    useEffect(() => {
        const handleOffline = () => setIsServerDown(true);
        window.addEventListener("offline-detected", handleOffline);
        return () => window.removeEventListener("offline-detected", handleOffline);
    }, []);

    if (isServerDown || window.location.pathname === "/offline") {
        return <OfflinePage />;
    }

    if (loading) return <LoadingScreen />;

    return (
        <>
            <NavbarSelector />
            <AppRoutes />
            <Footer />
        </>
    );
};

function App() {
    return (
        <BrowserRouter>
            <AuthProvider>
                <ScrollToTop />
                <AppContent />
            </AuthProvider>
        </BrowserRouter>
    );
}

export default App
