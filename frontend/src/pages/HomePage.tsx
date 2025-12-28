import PublicNavbar from "../components/navbar/PublicNavbar.tsx";
import HeroSection from "../components/home/HeroSection.tsx";
import Footer from "../components/common/Footer.tsx";
function HomePage() {
    return (
        <>
            <PublicNavbar/>
            <HeroSection />
            <Footer />
        </>
    )
}

export default HomePage;